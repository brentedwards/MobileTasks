package com.magenic.mobiletasks.mobiletasksandroid;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.CheckBox;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonObject;
import com.magenic.mobiletasks.mobiletasksandroid.R;
import com.magenic.mobiletasks.mobiletasksandroid.constants.IntentConstants;
import com.magenic.mobiletasks.mobiletasksandroid.constants.NetworkConstants;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;
import com.magenic.mobiletasks.mobiletasksandroid.services.NetworkService;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

public class TaskDetailActivity extends ActivityBase implements View.OnClickListener {

    private MobileTask task;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.task_detail);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        task = new MobileTask();

        ImageButton changeStartDate = (ImageButton)findViewById(R.id.btnChangeDate);

        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fabSaveTask);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                saveTask();
            }
        });

        RecyclerView lstTasks = (RecyclerView)this.findViewById(R.id.lstTasks);

        changeStartDate.setOnClickListener(this);
    }

    private void saveTask() {
        final ActivityBase context = this;
        CheckBox completed = (CheckBox)findViewById(R.id.completed);
        EditText description = (EditText)findViewById(R.id.taskDescription);
        task.isCompleted = completed.isChecked();
        task.setDescription(description.getText().toString());

        ListenableFuture<MobileTask> upsertFuture = this.networkService.upsertTask(task);
        Futures.addCallback(upsertFuture, new FutureCallback<MobileTask>() {
            @Override
            public void onSuccess(MobileTask returnedTask) {
                GsonBuilder gsonb = new GsonBuilder();
                gsonb.setDateFormat(NetworkConstants.DateFormat);
                Gson gson = gsonb.create();

                JsonObject task = networkService.serializeTask(gson, returnedTask);
                Intent myIntent = new Intent(context, TasksActivity.class);
                myIntent.putExtra(IntentConstants.NewTaskKey, task.getAsString());
                context.setResult(RESULT_OK, myIntent);
                context.finish();
            }

            @Override
            public void onFailure(Throwable t) {
                context.handleNetworkCallError(t);
            }
        });
    }

    @Override
    public void onClick(View v) {
        if (v.getId() == R.id.btnChangeDate) {
            final TextView currentDate = (TextView)findViewById(R.id.currentDate);
            final MobileTask curTask = task;
            int year;
            int month;
            int day;
            Calendar calendar = Calendar.getInstance();

            if (curTask.dateDue != null) {
                calendar.setTime(curTask.dateDue);
            }

            year = calendar.get(Calendar.YEAR);
            month = calendar.get(Calendar.MONTH);
            day = calendar.get(Calendar.DAY_OF_MONTH);

            DatePickerDialog dpd = new DatePickerDialog(this,
                    new DatePickerDialog.OnDateSetListener() {

                        @Override
                        public void onDateSet(DatePicker view, int year,
                                              int monthOfYear, int dayOfMonth) {
                            try {
                                SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy");
                                String textDate = (monthOfYear + 1) + "/" + dayOfMonth + "/" + year;
                                curTask.setDateDue(sdf.parse(textDate));
                                currentDate.setText(textDate);
                            } catch (ParseException e) {
                                // Production code would do something here
                            }
                        }
                    }, year, month, day);
            dpd.show();
        }
    }
}

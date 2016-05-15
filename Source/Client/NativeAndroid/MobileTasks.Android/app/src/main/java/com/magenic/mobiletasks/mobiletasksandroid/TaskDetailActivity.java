package com.magenic.mobiletasks.mobiletasksandroid;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.CompoundButton;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Switch;
import android.widget.ImageButton;
import android.widget.CompoundButton.OnCheckedChangeListener;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonObject;
import com.magenic.mobiletasks.mobiletasksandroid.constants.IntentConstants;
import com.magenic.mobiletasks.mobiletasksandroid.constants.NetworkConstants;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;

public class TaskDetailActivity extends ActivityBase implements View.OnClickListener, OnCheckedChangeListener {

    private MobileTask task;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.task_detail);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setDisplayShowHomeEnabled(true);

        task = new MobileTask();

        ImageButton changeStartDate = (ImageButton)findViewById(R.id.changeDate);
        changeStartDate.setOnClickListener(this);
        changeStartDate.setEnabled(false);
        changeStartDate.setClickable(false);

        Switch canChangeDate = (Switch)findViewById(R.id.canChangeDate);
        canChangeDate.setOnCheckedChangeListener(this);
    }

    private void saveTask() {
        final ActivityBase context = this;
        Switch completed = (Switch)findViewById(R.id.completed);
        EditText description = (EditText)findViewById(R.id.description);
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

                Intent myIntent = new Intent(context, MainActivity.class);
                myIntent.putExtra(IntentConstants.NewTaskKey, gson.toJson(task));
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
    public boolean onCreateOptionsMenu(Menu menu) {
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.detail_menu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.save:
                saveTask();
                return true;
            case android.R.id.home:
                finish();
                return true;
            default:
                // If we got here, the user's action was not recognized.
                // Invoke the superclass to handle it.
                return super.onOptionsItemSelected(item);
        }
    }

    @Override
    public void onClick(View v) {
        if (v.getId() == R.id.changeDate) {
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

    @Override
    public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
        ImageButton changeStartDate = (ImageButton)findViewById(R.id.changeDate);
        Switch canChangeDate = (Switch)findViewById(R.id.canChangeDate);

        changeStartDate.setEnabled(canChangeDate.isChecked());
        changeStartDate.setClickable(canChangeDate.isChecked());
    }
}

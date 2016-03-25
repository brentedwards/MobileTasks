package com.magenic.mobiletasks.mobiletasksandroid;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.View;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.google.gson.JsonSerializer;
import com.google.gson.stream.JsonReader;
import com.magenic.mobiletasks.mobiletasksandroid.R;
import com.magenic.mobiletasks.mobiletasksandroid.adapters.TaskAdapter;
import com.magenic.mobiletasks.mobiletasksandroid.constants.IntentConstants;
import com.magenic.mobiletasks.mobiletasksandroid.constants.NetworkConstants;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;
import com.magenic.mobiletasks.mobiletasksandroid.services.NetworkService;
import com.microsoft.windowsazure.mobileservices.MobileServiceException;

import java.util.ArrayList;
import java.util.List;

public class TasksActivity extends ActivityBase {

    private List<MobileTask> tasks;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.tasks);
        final AppCompatActivity context = this;
        // Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        //setSupportActionBar(toolbar);

        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fabCreateTask);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivityForResult(new Intent(context, TaskDetailActivity.class), IntentConstants.NewTaskResult);
            }
        });

        RecyclerView lstTasks = (RecyclerView)this.findViewById(R.id.lstTasks);

        lstTasks.setLayoutManager(new LinearLayoutManager(this));

        lstTasks.setAdapter(new TaskAdapter(this, new ArrayList<MobileTask>(), networkService));
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (tasks == null) {
            final ActivityBase context = this;
            ListenableFuture<List<MobileTask>> tasksFuture = networkService.getTasks();

            Futures.addCallback(tasksFuture, new FutureCallback<List<MobileTask>>() {
                @Override
                public void onSuccess(List<MobileTask> returnedTasks) {
                    final List<MobileTask> tasks = returnedTasks;
                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            resetList(tasks);
                        }
                    });
                }

                @Override
                public void onFailure(Throwable t) {
                    context.handleNetworkCallError(t);
                }
            });
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        // Check which request we're responding to
        if (requestCode == IntentConstants.NewTaskResult) {
            // Make sure the request was successful
            if (resultCode == RESULT_OK) {
                if (this.tasks != null) {
                    String newTask = data.getStringExtra(IntentConstants.NewTaskKey);
                    JsonParser parser = new JsonParser();
                    JsonObject tasko = parser.parse(newTask).getAsJsonObject();

                    GsonBuilder gsonb = new GsonBuilder();
                    gsonb.setDateFormat(NetworkConstants.DateFormat);
                    Gson gson = gsonb.create();

                    MobileTask task = this.networkService.deserializeTask(gson, tasko);

                    tasks.add(task);
                }
            }
        }
    }

    private void resetList(List<MobileTask> tasks) {
        RecyclerView lstRegistrations = (RecyclerView)this.findViewById(R.id.lstTasks);
        if (lstRegistrations != null) {
            ((TaskAdapter)lstRegistrations.getAdapter()).setTasks(tasks);
            ((TaskAdapter) lstRegistrations.getAdapter()).notifyDataSetChanged();
        }
    }
}
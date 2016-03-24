package com.magenic.mobiletasks.mobiletasksandroid;

import android.content.Intent;
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
import com.magenic.mobiletasks.mobiletasksandroid.R;
import com.magenic.mobiletasks.mobiletasksandroid.adapters.TaskAdapter;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;
import com.magenic.mobiletasks.mobiletasksandroid.services.NetworkService;

import java.util.ArrayList;
import java.util.List;

public class TasksActivity extends AppCompatActivity {

    private List<MobileTask> tasks;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.tasks);
        final AppCompatActivity context = this;
        // Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        //setSupportActionBar(toolbar);

        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivity(new Intent(context, TaskDetailActivity.class));
            }
        });

        RecyclerView lstTasks = (RecyclerView)this.findViewById(R.id.lstTasks);
        lstTasks.setLayoutManager(new LinearLayoutManager(this));

        INetworkService networkSerice = new NetworkService();
        networkSerice.setContext(this);
        lstTasks.setAdapter(new TaskAdapter(this, new ArrayList<MobileTask>(), networkSerice));
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (tasks == null) {
            INetworkService networkSerice = new NetworkService();
            networkSerice.setContext(this);
            ListenableFuture<List<MobileTask>> tasksFuture = networkSerice.getTasks();
            final AppCompatActivity context = this;

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
                    AlertDialog.Builder dlgAlert  = new AlertDialog.Builder(context);
                    dlgAlert.setTitle("Tasks Failure");
                    dlgAlert.setMessage("The following error occurred returning the task list: " + t.getMessage());
                    dlgAlert.setPositiveButton("OK", null);
                    dlgAlert.setCancelable(true);
                    dlgAlert.create().show();
                }
            });
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
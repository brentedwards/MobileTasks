package com.magenic.mobiletasks.mobiletasksandroid;

import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.magenic.mobiletasks.mobiletasksandroid.R;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;
import com.magenic.mobiletasks.mobiletasksandroid.services.NetworkService;

import java.util.List;

public class TasksActivity extends AppCompatActivity {

    private List<MobileTask> tasks;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.tasks);
        // Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        //setSupportActionBar(toolbar);

        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
            }
        });
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
                    tasks = returnedTasks;
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
}
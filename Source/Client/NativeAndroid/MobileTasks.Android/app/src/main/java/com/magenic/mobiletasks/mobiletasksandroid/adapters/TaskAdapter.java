package com.magenic.mobiletasks.mobiletasksandroid.adapters;

import android.content.Context;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.CompoundButton;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.magenic.mobiletasks.mobiletasksandroid.R;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;
import com.magenic.mobiletasks.mobiletasksandroid.viewholders.TaskViewHolder;

import java.util.List;

/**
 * Created by kevinf on 3/23/2016.
 */
public class TaskAdapter extends RecyclerView.Adapter<TaskViewHolder>  implements CheckBox.OnCheckedChangeListener {
    private Context context;
    private List<MobileTask> tasks;
    private INetworkService networkService;

    public TaskAdapter(Context context, List<MobileTask> tasks, INetworkService networkService)
    {
        this.context = context;
        this.tasks = tasks;
        this.networkService = networkService;
    }

    public final void setTasks(List<MobileTask> tasks) {
        this.tasks = tasks;
    }

    @Override
    public TaskViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.taskrow, parent, false);

        TaskViewHolder viewHolder = new TaskViewHolder(view);
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(TaskViewHolder viewHolder, int i) {
        MobileTask entry = tasks.get(i);

        String desicription = entry.getDescription();
        if (desicription.length() > 30) {
            desicription = desicription.substring(1, 30);
        }
        viewHolder.taskCompleted.setText(desicription);
        viewHolder.taskCompleted.setChecked(entry.getIsCompleted());
        viewHolder.taskCompleted.setOnCheckedChangeListener(this);
        viewHolder.taskCompleted.setTag(viewHolder);
        viewHolder.index = i;
    }

    @Override
    public int getItemCount() {
        return tasks.size();
    }

    public MobileTask getItem(int item) {
        return tasks.get(item);
    }

    @Override
    public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
        MobileTask task = this.getItem(((TaskViewHolder)buttonView.getTag()).index);
        task.isCompleted = isChecked;
        ListenableFuture<MobileTask> upsertFuture = this.networkService.upsertTask(task);
        Futures.addCallback(upsertFuture, new FutureCallback<MobileTask>() {
            @Override
            public void onSuccess(MobileTask returnedTask) {
            }

            @Override
            public void onFailure(Throwable t) {
                AlertDialog.Builder dlgAlert = new AlertDialog.Builder(context);
                dlgAlert.setTitle("Task update Failure");
                dlgAlert.setMessage("The following error occurred updating the task: " + t.getMessage());
                dlgAlert.setPositiveButton("OK", null);
                dlgAlert.setCancelable(true);
                dlgAlert.create().show();
            }
        });
    }
}
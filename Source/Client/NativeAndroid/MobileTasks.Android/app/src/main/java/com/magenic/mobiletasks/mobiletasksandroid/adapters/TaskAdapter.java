package com.magenic.mobiletasks.mobiletasksandroid.adapters;

import android.content.Context;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.ImageView;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.magenic.mobiletasks.mobiletasksandroid.R;
import com.magenic.mobiletasks.mobiletasksandroid.constants.NetworkConstants;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;
import com.magenic.mobiletasks.mobiletasksandroid.viewholders.TaskViewHolder;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.List;
import java.util.Date;
import java.util.TimeZone;

/**
 * Created by kevinf on 3/23/2016.
 */
public class TaskAdapter extends RecyclerView.Adapter<TaskViewHolder>  implements ImageView.OnClickListener {
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
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.taskview, parent, false);

        TaskViewHolder viewHolder = new TaskViewHolder(view);
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(TaskViewHolder viewHolder, int i) {
        MobileTask entry = tasks.get(i);

        String description = entry.getDescription();
        if (description.length() > 30) {
            description = description.substring(1, 30);
        }
        Integer statusResourceId = this.getStatusResourceId(entry.isCompleted, entry.getDateDue());

        String formattedDate = "";

        if (entry.getDateDue() != null) {
            SimpleDateFormat sdf = new SimpleDateFormat(NetworkConstants.DisplayDateFormat);
            formattedDate = sdf.format(entry.getDateDue());
        }

        viewHolder.taskStatus.setOnClickListener(this);
        viewHolder.taskStatus.setTag(viewHolder);
        viewHolder.taskDescription.setText(description);
        viewHolder.taskStatus.setImageResource(statusResourceId);
        viewHolder.taskDateDue.setText(formattedDate);
        viewHolder.taskInformation.setOnClickListener(this);
        viewHolder.taskInformation.setTag(viewHolder);
        viewHolder.index = i;
    }

    private Integer getStatusResourceId(boolean isCompleted, Date dateDue) {
        if (isCompleted) {
            return R.drawable.iconcompleted;
        } else if (dateDue != null && dateDue.before(new Date())) {
            return R.drawable.iconpastdue;
        } else {
            return R.drawable.iconincomplete;
        }
    }

    @Override
    public int getItemCount() {
        return tasks.size();
    }

    public MobileTask getItem(int item) {
        return tasks.get(item);
    }

    @Override
    public void onClick(View v) {
        if (v.getId() == R.id.state) {
            final ImageView imageView = (ImageView)v;
            MobileTask task = this.getItem(((TaskViewHolder)v.getTag()).index);
            if (!task.isCompleted) {
                task.isCompleted = true;
                ListenableFuture<MobileTask> upsertFuture = this.networkService.upsertTask(task);
                Futures.addCallback(upsertFuture, new FutureCallback<MobileTask>() {
                    @Override
                    public void onSuccess(MobileTask returnedTask) {
                        imageView.setImageResource(getStatusResourceId(returnedTask.isCompleted, returnedTask.getDateDue()));
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
        } else if (v.getId() == R.id.information) {
            //final MobileTask task = this.getItem(((TaskViewHolder)v.getTag()).index);
            //ListenableFuture<MobileTask> upsertFuture = this.networkService.delete(String.valueOf(task.getId()));
            //Futures.addCallback(upsertFuture, new FutureCallback<MobileTask>() {
            //    @Override
            //    public void onSuccess(MobileTask returnedTask) {
            //        tasks.remove(task);
            //    }

            //    @Override
            //    public void onFailure(Throwable t) {
            //        AlertDialog.Builder dlgAlert = new AlertDialog.Builder(context);
            //        dlgAlert.setTitle("Task update Failure");
            //        dlgAlert.setMessage("The following error occurred updating the task: " + t.getMessage());
            //        dlgAlert.setPositiveButton("OK", null);
            //        dlgAlert.setCancelable(true);
            //        dlgAlert.create().show();
            //    }
            //});
        }
    }
}
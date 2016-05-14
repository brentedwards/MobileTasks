package com.magenic.mobiletasks.mobiletasksandroid.viewholders;

import android.widget.ImageView;
import android.support.v7.widget.CardView;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.TextView;
import android.widget.RelativeLayout;

import com.magenic.mobiletasks.mobiletasksandroid.R;

/**
 * Created by kevinf on 3/23/2016.
 */
public class TaskViewHolder extends RecyclerView.ViewHolder {
    public ImageView taskStatus;
    public TextView taskDescription;
    public TextView taskDateDue;
    public RelativeLayout taskRow;
    public ImageView taskInformation;
    public int index;

    public TaskViewHolder(View itemView) {
        super(itemView);

        taskStatus = (ImageView)itemView.findViewById(R.id.state);
        taskRow = (RelativeLayout)itemView.findViewById(R.id.taskRow);
        taskDescription = (TextView)itemView.findViewById(R.id.description);
        taskDateDue = (TextView)itemView.findViewById(R.id.dateDue);
        taskInformation = (ImageView)itemView.findViewById(R.id.information);
        index = 0;
    }
}

package com.magenic.mobiletasks.mobiletasksandroid.viewholders;

import android.support.v7.widget.CardView;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.CheckBox;

import com.magenic.mobiletasks.mobiletasksandroid.R;

/**
 * Created by kevinf on 3/23/2016.
 */
public class TaskViewHolder extends RecyclerView.ViewHolder {
    public CheckBox taskCompleted;
    public CardView taskRow;
    public int index;

    public TaskViewHolder(View itemView) {
        super(itemView);

        taskCompleted = (CheckBox)itemView.findViewById(R.id.taskCompleted);
        taskRow = (CardView)itemView.findViewById(R.id.taskRow);
        index = 0;
    }
}

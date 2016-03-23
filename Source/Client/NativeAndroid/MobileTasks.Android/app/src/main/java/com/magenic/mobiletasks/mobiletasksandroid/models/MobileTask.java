package com.magenic.mobiletasks.mobiletasksandroid.models;

import com.google.gson.annotations.SerializedName;

import java.util.Date;

/**
 * Created by kevinf on 3/22/2016.
 */
public class MobileTask {
    @SerializedName("Id")
    public int id;

    public int getId() {
        return id;
    }

    public void setid(int id) {
        this.id = id;
    }

    @SerializedName("Sid")
    public String sid;

    public String getSid() {
        return sid;
    }

    public void setSid(String sid) {
        this.sid = sid;
    }

    @SerializedName("DateCreated")
    public Date dateCreated;

    public Date getDateCreated() {
        return dateCreated;
    }

    public void setDateCreated(Date dateCreated) {
        this.dateCreated = dateCreated;
    }

    @SerializedName("DateCompleted")
    public Date dateCompleted;

    public Date getDateCompleted() {
        return dateCompleted;
    }

    public void setDateCompleted(Date dateCompleted) {
        this.dateCompleted = dateCompleted;
    }

    @SerializedName("Description")
    public String description;

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    @SerializedName("IsCompleted")
    public boolean isCompleted;

    public boolean getIsCompleted() {
        return isCompleted;
    }

    public void setIsCompleted(boolean isCompleted) {
        this.isCompleted = isCompleted;
    }

    @SerializedName("DateDue")
    public Date dateDue;

    public Date getDateDue() {
        return dateDue;
    }

    public void setDateDue(Date dateDue) {
        this.dateDue = dateDue;
    }
}

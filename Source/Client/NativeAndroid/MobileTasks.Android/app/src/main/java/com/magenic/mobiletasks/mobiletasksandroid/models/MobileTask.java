package com.magenic.mobiletasks.mobiletasksandroid.models;

import com.google.gson.annotations.SerializedName;

import java.util.Date;

/**
 * Created by kevinf on 3/22/2016.
 */
public class MobileTask {
    @SerializedName("id")
    public int id;

    public int getId() {
        return id;
    }

    public void setid(int id) {
        this.id = id;
    }

    @SerializedName("sid")
    public String sid;

    public String getSid() {
        return sid;
    }

    public void setSid(String sid) {
        this.sid = sid;
    }

    @SerializedName("dateCreated")
    public Date dateCreated;

    public Date getDateCreated() {
        return dateCreated;
    }

    public void setDateCreated(Date dateCreated) {
        this.dateCreated = dateCreated;
    }

    @SerializedName("dateCompleted")
    public Date dateCompleted;

    public Date getDateCompleted() {
        return dateCompleted;
    }

    public void setDateCompleted(Date dateCompleted) {
        this.dateCompleted = dateCompleted;
    }

    @SerializedName("description")
    public String description;

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    @SerializedName("isCompleted")
    public boolean isCompleted;

    public boolean getIsCompleted() {
        return isCompleted;
    }

    public void setIsCompleted(boolean isCompleted) {
        this.isCompleted = isCompleted;
    }

    @SerializedName("dateDue")
    public Date dateDue;

    public Date getDateDue() {
        return dateDue;
    }

    public void setDateDue(Date dateDue) {
        this.dateDue = dateDue;
    }
}

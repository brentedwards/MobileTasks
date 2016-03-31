package com.magenic.mobiletasks.mobiletasksandroid.interfaces;

import android.app.Activity;

import com.google.common.util.concurrent.ListenableFuture;
import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;
import com.microsoft.windowsazure.mobileservices.MobileServiceClient;

import java.util.List;

/**
 * Created by kevinf on 3/21/2016.
 */
public interface INetworkService {
    MobileServiceClient getClient();
    void setContext(Activity context);
    ListenableFuture<List<MobileTask>> getTasks();
    ListenableFuture<MobileTask> upsertTask(MobileTask task);
    MobileTask deserializeTask(Gson gson, JsonObject element);
    JsonObject serializeTask(Gson gson, MobileTask task);
    ListenableFuture logout();
    ListenableFuture delete(String id);
}
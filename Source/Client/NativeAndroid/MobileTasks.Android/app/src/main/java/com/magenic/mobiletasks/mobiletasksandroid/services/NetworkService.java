package com.magenic.mobiletasks.mobiletasksandroid.services;

import android.app.Activity;
import android.util.Log;
import android.widget.Toast;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.google.common.util.concurrent.SettableFuture;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;
import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.http.HttpConstants;

import java.net.MalformedURLException;
import java.util.ArrayList;
import java.util.List;

/**
 * Created by kevinf on 3/22/2016.
 */
public class NetworkService implements INetworkService {

    private static MobileServiceClient client;
    private String appUrl = "https://mobiletasks.azurewebsites.net";

    @Override
    public MobileServiceClient getClient() {
        // Productions services might throw exception if client is null.
        return client;
    }

    @Override
    public void setContext(Activity context) {
        try {
            if (client == null) {
                client = new MobileServiceClient(appUrl,  context);
            } else {
                MobileServiceClient newClient = new MobileServiceClient(appUrl,  context);
                newClient.setCurrentUser(client.getCurrentUser());
                client = newClient;
            }
        }
        catch (MalformedURLException e) {
            // This should never happen; handle more robustly in production code
        }
    }

    @Override
    public ListenableFuture<List<MobileTask>> getTasks() {
        final SettableFuture<List<MobileTask>> result = SettableFuture.create();
        ListenableFuture tasksFuture = client.invokeApi("task", HttpConstants.GetMethod, JsonElement.class);
        Futures.addCallback(tasksFuture, new FutureCallback<JsonElement>() {
            @Override
            public void onSuccess(JsonElement tasks) {
                GsonBuilder gsonb = new GsonBuilder();
                Gson gson = gsonb.create();

                JsonArray array = tasks.getAsJsonArray();
                List<MobileTask> taskList = new ArrayList<MobileTask>();
                for (int i = 0; i < array.size(); i++) {
                    taskList.add(gson.fromJson(array.get(i).getAsJsonObject().toString(), MobileTask.class));
                }
                result.set(taskList);
            }

            @Override
            public void onFailure(Throwable t) {
                result.setException(t);
            }
        });
        return result;
    }
}
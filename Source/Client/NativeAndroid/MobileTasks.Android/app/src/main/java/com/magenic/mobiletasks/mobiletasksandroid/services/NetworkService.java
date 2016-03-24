package com.magenic.mobiletasks.mobiletasksandroid.services;

import android.app.Activity;
import android.util.Pair;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.google.common.util.concurrent.SettableFuture;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.magenic.mobiletasks.mobiletasksandroid.constants.NetworkConstants;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.models.MobileTask;
import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.http.HttpConstants;

import java.net.MalformedURLException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.TimeZone;

/**
 * Created by kevinf on 3/22/2016.
 */
public class NetworkService implements INetworkService {

    private static MobileServiceClient client;

    @Override
    public MobileServiceClient getClient() {
        // Productions services might throw exception if client is null.
        return client;
    }

    @Override
    public void setContext(Activity context) {
        try {
            if (client == null) {
                client = new MobileServiceClient(NetworkConstants.ServiceUrl,  context);
            } else {
                MobileServiceClient newClient = new MobileServiceClient(NetworkConstants.ServiceUrl,  context);
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
        List<Pair<String, String>> parameters = new ArrayList<Pair<String, String>>();
        ListenableFuture<JsonElement> tasksFuture = client.invokeApi("task","GET", parameters);
        Futures.addCallback(tasksFuture, new FutureCallback<JsonElement>() {
            @Override
            public void onSuccess(JsonElement tasks) {
                GsonBuilder gsonb = new GsonBuilder();
                gsonb.setDateFormat(NetworkConstants.DateFormat);
                Gson gson = gsonb.create();

                JsonArray array = tasks.getAsJsonArray();
                List<MobileTask> taskList = new ArrayList<MobileTask>();
                for (int i = 0; i < array.size(); i++) {
                    JsonObject element = array.get(i).getAsJsonObject();

                    MobileTask task = deserializeTask(gson, element);

                    taskList.add(task);
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

    private MobileTask deserializeTask(Gson gson, JsonObject element) {
        element = reformatDateString(element, "dateCreated");
        element = reformatDateString(element, "dateDue");
        element = reformatDateString(element, "dateCompleted");


        return gson.fromJson(element.toString(), MobileTask.class);
    }

    private JsonObject serializeTask(Gson gson, MobileTask task) {
        JsonObject element = gson.toJsonTree(task).getAsJsonObject();

        element = formatDateString(element, "dateCreated", gson);
        element = formatDateString(element, "dateDue", gson);
        element = formatDateString(element, "dateCompleted", gson);

        return element;
    }

    @Override
    public ListenableFuture<MobileTask> upsertTask(MobileTask task) {
        final SettableFuture<MobileTask> result = SettableFuture.create();
        List<Pair<String, String>> parameters = new ArrayList<Pair<String, String>>();
        GsonBuilder gsonb = new GsonBuilder();
        gsonb.setDateFormat(NetworkConstants.DateFormat);

        final Gson gson = gsonb.create();

        JsonObject element = serializeTask(gson, task);

        ListenableFuture<JsonElement> tasksFuture = client.invokeApi("task", element);
        Futures.addCallback(tasksFuture, new FutureCallback<JsonElement>() {
            @Override
            public void onSuccess(JsonElement task) {
                result.set(deserializeTask(gson, task.getAsJsonObject()));
            }

            @Override
            public void onFailure(Throwable t) {
                result.setException(t);
            }
        });

        return result;
    }

    private JsonObject reformatDateString(JsonObject element, String  propertyName) {
        if (!element.get(propertyName).isJsonNull()) {
            String curDate = element.get(propertyName).getAsString();
            curDate = curDate.replaceAll("Z", "UTC");
            element.remove(propertyName);
            element.addProperty(propertyName, curDate);
        }
        return element;
    }

    private JsonObject formatDateString(JsonObject element, String  propertyName, Gson gson) {
        if (element.get(propertyName)!= null) {
            try {
                String curDate = element.get(propertyName).getAsString();
                SimpleDateFormat sdf = new SimpleDateFormat(NetworkConstants.DateFormat);
                Date javaDate = sdf.parse(curDate);

                sdf.setTimeZone(TimeZone.getTimeZone("gmt"));
                String gmtTime = sdf.format(javaDate);

                gmtTime = gmtTime.replaceAll("GMT", "Z");
                element.remove(propertyName);
                element.addProperty(propertyName, gmtTime);
            } catch (ParseException e) {
                // Production code would do something here
            }

        } else {
            element.addProperty(propertyName, "null");
        }
        return element;
    }
}
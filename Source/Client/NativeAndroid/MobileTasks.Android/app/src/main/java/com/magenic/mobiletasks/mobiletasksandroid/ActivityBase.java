package com.magenic.mobiletasks.mobiletasksandroid;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.magenic.mobiletasks.mobiletasksandroid.constants.NetworkConstants;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.services.NetworkService;
import com.microsoft.windowsazure.mobileservices.MobileServiceException;

/**
 * Created by kevinf on 3/24/2016.
 */
public class ActivityBase extends AppCompatActivity {
    protected INetworkService networkService = new NetworkService();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        networkService = new NetworkService();
        networkService.setContext(this);
    }

    protected void showMessage(String title, String message) {
        AlertDialog.Builder dlgAlert  = new AlertDialog.Builder(this);
        dlgAlert.setTitle(title);
        dlgAlert.setMessage(message);
        dlgAlert.setPositiveButton("OK", null);
        dlgAlert.setCancelable(true);
        dlgAlert.create().show();
    }

    protected void handleNetworkCallError(Throwable t) {
        if (t instanceof MobileServiceException && ((MobileServiceException) t).getResponse().getStatus().message.equals("Unauthorized")) {
            final ActivityBase context = this;
            ListenableFuture tasksFuture = networkService.logout();
            Futures.addCallback(tasksFuture, new FutureCallback() {
                @Override
                public void onSuccess(Object result) {
                    SharedPreferences prefs = context.getSharedPreferences(context.getApplicationContext().getPackageName(), 0);
                    SharedPreferences.Editor editor = prefs.edit();
                    editor.remove(NetworkConstants.LastUserProvider);
                    editor.commit();

                    Intent intent = new Intent(context, LoginActivity.class);
                    intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
                    startActivity(intent);
                    finish();
                }

                @Override
                public void onFailure(Throwable t) {
                }
            });
        } else {
            this.showMessage("Error", "The following error occurred calling the server: " + t.getMessage());
        }
    }
}

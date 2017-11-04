package com.magenic.mobiletasks.mobiletasksandroid.models;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.google.common.util.concurrent.SettableFuture;
import com.magenic.mobiletasks.mobiletasksandroid.constants.NetworkConstants;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceAuthenticationProvider;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceUser;

import android.app.Activity;
import android.content.SharedPreferences;

import java.net.MalformedURLException;

/**
 * Created by kevinf on 3/21/2016.
 */
public class Login {

    private INetworkService networkSerice;
    private Activity currentContext;
    private String userId = "|UserId";
    private String token = "|Token";


    public Login(Activity context, INetworkService networkService) {
        currentContext = context;
        networkSerice = networkService;
    }

    public boolean checkPreviousAuthentication()
    {
        SharedPreferences prefs = currentContext.getSharedPreferences(currentContext.getApplicationContext().getPackageName(), 0);

        String userProviderType = prefs.getString(NetworkConstants.LastUserProvider, "");

        if (!userProviderType.equals("")) {
            String currentUserId = prefs.getString(userProviderType + userId, "");
            String currentToken = prefs.getString(userProviderType + token, "");

            if (!currentUserId.equals("")) {
                networkSerice.getClient().setCurrentUser(new MobileServiceUser(userId));
                networkSerice.getClient().getCurrentUser().setAuthenticationToken(currentToken);
                return true;
            }
        }
        return false;
    }

    public void authenticate(MobileServiceAuthenticationProvider provider, int loginRequestCode) {

        networkSerice.getClient().login(provider, "commagenicmobiletasks", loginRequestCode);
    }

    public void setToken(MobileServiceAuthenticationProvider requestedProvider) {
        String currentToken = networkSerice.getClient().getCurrentUser().getAuthenticationToken();
        String currentUserId = networkSerice.getClient().getCurrentUser().getUserId();

        SharedPreferences prefs = currentContext.getSharedPreferences(currentContext.getApplicationContext().getPackageName(), 0);
        SharedPreferences.Editor editor = prefs.edit();
        editor.putString(NetworkConstants.LastUserProvider, requestedProvider.toString());
        editor.putString(requestedProvider.toString() + userId, currentUserId);
        editor.putString(requestedProvider.toString() + token, currentToken);
        editor.commit();
    }
}
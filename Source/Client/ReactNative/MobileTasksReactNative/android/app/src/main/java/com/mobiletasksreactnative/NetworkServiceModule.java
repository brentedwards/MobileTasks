package com.mobiletasksreactnative;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;

import com.facebook.react.bridge.ReactApplicationContext;
import com.facebook.react.bridge.ReactContextBaseJavaModule;
import com.facebook.react.bridge.ActivityEventListener;
import com.facebook.react.bridge.BaseActivityEventListener;
import com.facebook.react.bridge.ReactMethod;
import com.facebook.react.bridge.Callback;
import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.microsoft.windowsazure.mobileservices.MobileServiceActivityResult;
import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceAuthenticationProvider;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceUser;

import java.net.MalformedURLException;



/**
 * Created by kevinford on 11/1/17.
 */

public class NetworkServiceModule extends ReactContextBaseJavaModule {

    private String userId = "|UserId";
    private String token = "|Token";

    private Callback _LoginCallback;
    private static MobileServiceClient client;
    private int LOGIN_REQUEST_CODE = 1;
    private MobileServiceAuthenticationProvider requestedProvider;

    public MobileServiceClient getClient() {
        Activity currentActivity = getCurrentActivity();
        if (client == null) {
            try {
                MobileServiceClient newClient = new MobileServiceClient(NetworkConstants.ServiceUrl,  currentActivity);
                client = newClient;
            }
            catch (MalformedURLException e) {
                // This should never happen; handle more robustly in production code
            }
        }
        return client;
    }

    private final ActivityEventListener mActivityEventListener = new BaseActivityEventListener() {

        @Override
        public void onActivityResult(Activity activity, int requestCode, int resultCode, Intent intent) {
            if (resultCode ==  Activity.RESULT_OK) {
                // Check the request code matches the one we send in the login request
                if (requestCode == LOGIN_REQUEST_CODE) {
                    MobileServiceActivityResult result = getClient().onActivityResult(intent);
                    if (result.isLoggedIn()) {
                        setToken(requestedProvider);
                        _LoginCallback.invoke("");
                    } else {
                        // login failed, check the error message
                        _LoginCallback.invoke("No Logged In");
                    }
                }
            } else {
                _LoginCallback.invoke("Error logging in");
            }
        }
    };

    public NetworkServiceModule(ReactApplicationContext reactContext) {
        super(reactContext);

        // Add the listener for `onActivityResult`
        reactContext.addActivityEventListener(mActivityEventListener);
    }

    @Override
    public String getName() {
        return "NetworkService";
    }

    @ReactMethod
    public void hasPreviousAuthentication(Callback callBack) {
        SharedPreferences prefs = getCurrentActivity().getSharedPreferences(getCurrentActivity().getApplicationContext().getPackageName(), 0);

        String userProviderType = prefs.getString(NetworkConstants.LastUserProvider, "");

        if (!userProviderType.equals("")) {
            String currentUserId = prefs.getString(userProviderType + userId, "");
            String currentToken = prefs.getString(userProviderType + token, "");

            if (!currentUserId.equals("")) {
                getClient().setCurrentUser(new MobileServiceUser(userId));
                getClient().getCurrentUser().setAuthenticationToken(currentToken);
                callBack.invoke("", "true");
            }
        }
        callBack.invoke("", "false");
    }

    @ReactMethod
    public void login(String serviceProvider, Callback callBack) {
        _LoginCallback = callBack;

        requestedProvider = getProvider(serviceProvider);
        getClient().login(requestedProvider, "commagenicmobiletasks", LOGIN_REQUEST_CODE);
    }

    @ReactMethod
    public void getUserInfo(Callback callBack) {
        if (getClient().getCurrentUser() != null) {
            MobileServiceUser user = getClient().getCurrentUser();
            callBack.invoke(user.getAuthenticationToken(), user.getUserId());
        } else {
            callBack.invoke("", "");
        }
    }

    @ReactMethod
    public void logout(final Callback callBack) {
        ListenableFuture logoutFuture = getClient().logout();

        Futures.addCallback(logoutFuture, new FutureCallback<Object>() {
            @Override
            public void onSuccess(Object result) {
                SharedPreferences prefs = getCurrentActivity().getSharedPreferences(getCurrentActivity().getApplicationContext().getPackageName(), 0);
                SharedPreferences.Editor editor = prefs.edit();
                editor.remove(NetworkConstants.LastUserProvider);
                editor.commit();
                callBack.invoke("");
            }

            @Override
            public void onFailure(Throwable t) {
                callBack.invoke(t.getMessage());
            }
        });
    }

    private MobileServiceAuthenticationProvider getProvider(String provierName) {
        switch (provierName.toUpperCase()) {
            case "GOOGLE":
                return MobileServiceAuthenticationProvider.Google;
            case "MICROSOFTACCOUNT":
                return MobileServiceAuthenticationProvider.MicrosoftAccount;
            case "FACEBOOK":
                return MobileServiceAuthenticationProvider.Facebook;
            case "TWITTER":
                return MobileServiceAuthenticationProvider.Twitter;
            default:
                return MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory;
        }
    }

    public void setToken(MobileServiceAuthenticationProvider requestedProvider) {
        String currentToken = getClient().getCurrentUser().getAuthenticationToken();
        String currentUserId = getClient().getCurrentUser().getUserId();

        SharedPreferences prefs = getCurrentActivity().getSharedPreferences(getCurrentActivity().getApplicationContext().getPackageName(), 0);
        SharedPreferences.Editor editor = prefs.edit();
        editor.putString(NetworkConstants.LastUserProvider, requestedProvider.toString());
        editor.putString(requestedProvider.toString() + userId, currentUserId);
        editor.putString(requestedProvider.toString() + token, currentToken);
        editor.commit();
    }
}
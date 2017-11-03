package com.magenic.mobiletasks.mobiletasksandroid;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageButton;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.magenic.mobiletasks.mobiletasksandroid.constants.NetworkConstants;
import com.magenic.mobiletasks.mobiletasksandroid.models.Login;
import com.microsoft.windowsazure.mobileservices.MobileServiceActivityResult;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceAuthenticationProvider;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceUser;
import com.microsoft.windowsazure.mobileservices.MobileServiceException;

public class LoginActivity extends ActivityBase implements View.OnClickListener {

    private Login login;
    public static final int LOGIN_REQUEST_CODE = 1;
    private MobileServiceAuthenticationProvider requestedProvider;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.login);

        login = new Login(this, networkService);

        ImageButton button = (ImageButton) this.findViewById(R.id.facebook);
        button.setOnClickListener(this);
        button = (ImageButton) this.findViewById(R.id.google);
        button.setOnClickListener(this);
        button = (ImageButton) this.findViewById(R.id.microsoft);
        button.setOnClickListener(this);
        button = (ImageButton) this.findViewById(R.id.twitter);
        button.setOnClickListener(this);
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (login.checkPreviousAuthentication()) {
            startActivity(new Intent(this, MainActivity.class));
        }
    }

    @Override
    public void onClick(View v) {
        MobileServiceAuthenticationProvider provider = MobileServiceAuthenticationProvider.MicrosoftAccount;
        switch (v.getId()) {
            case R.id.facebook:
                provider = MobileServiceAuthenticationProvider.Facebook;
                break;
            case R.id.google:
                provider = MobileServiceAuthenticationProvider.Google;
                break;
            case R.id.microsoft:
                provider = MobileServiceAuthenticationProvider.MicrosoftAccount;
                break;
            case R.id.twitter:
                provider = MobileServiceAuthenticationProvider.Twitter;
                break;
        }
        requestedProvider = provider;

        login.authenticate(provider, LOGIN_REQUEST_CODE);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        // When request completes
        if (resultCode == RESULT_OK) {
            // Check the request code matches the one we send in the login request
            if (requestCode == LOGIN_REQUEST_CODE) {
                MobileServiceActivityResult result = this.networkService.getClient().onActivityResult(data);
                if (result.isLoggedIn()) {
                    login.setToken(requestedProvider);
                    startActivity(new Intent(this, MainActivity.class));
                } else {
                    // login failed, check the error message

                }
            }
        } else {
            this.showMessage("Login Failure", "Error logging in");
        }
    }
}
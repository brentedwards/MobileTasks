package com.magenic.mobiletasks.mobiletasksandroid;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageButton;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.magenic.mobiletasks.mobiletasksandroid.models.Login;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceAuthenticationProvider;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceUser;
import com.microsoft.windowsazure.mobileservices.MobileServiceException;

public class LoginActivity extends ActivityBase implements View.OnClickListener {

    private Login login;

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
        final ActivityBase context = this;
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

        ListenableFuture loginFuture = login.authenticate(provider);

        Futures.addCallback(loginFuture, new FutureCallback<MobileServiceUser>() {
            @Override
            public void onSuccess(MobileServiceUser user) {
                startActivity(new Intent(context, MainActivity.class));
            }

            @Override
            public void onFailure(Throwable t) {
                if (t instanceof MobileServiceException && t.getCause().getMessage().equals("User Canceled")) {
                    return;
                }

                context.showMessage("Login Failure", "The following error occurred logging in: " + t.getMessage());
            }
        });
    }
}
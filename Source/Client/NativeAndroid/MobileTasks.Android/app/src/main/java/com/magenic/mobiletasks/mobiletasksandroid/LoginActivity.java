package com.magenic.mobiletasks.mobiletasksandroid;

import android.content.Intent;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.google.common.util.concurrent.FutureCallback;
import com.google.common.util.concurrent.Futures;
import com.google.common.util.concurrent.ListenableFuture;
import com.magenic.mobiletasks.mobiletasksandroid.interfaces.INetworkService;
import com.magenic.mobiletasks.mobiletasksandroid.models.Login;
import com.magenic.mobiletasks.mobiletasksandroid.services.NetworkService;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceAuthenticationProvider;
import com.microsoft.windowsazure.mobileservices.authentication.MobileServiceUser;
import com.microsoft.windowsazure.mobileservices.MobileServiceException;

public class LoginActivity extends AppCompatActivity implements View.OnClickListener {

    private Login login;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.login);

        INetworkService networkService = new NetworkService();
        networkService.setContext(this);

        login = new Login(this, networkService);

        Button button = (Button) this.findViewById(R.id.btnFacebook);
        button.setOnClickListener(this);
        button = (Button) this.findViewById(R.id.btnGoogle);
        button.setOnClickListener(this);
        button = (Button) this.findViewById(R.id.btnMicrosoft);
        button.setOnClickListener(this);
        button = (Button) this.findViewById(R.id.btnTwitter);
        button.setOnClickListener(this);
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (login.checkPreviousAuthentication()) {
            startActivity(new Intent(this, TasksActivity.class));
        }
    }

    @Override
    public void onClick(View v) {
        MobileServiceAuthenticationProvider provider = MobileServiceAuthenticationProvider.MicrosoftAccount;
        final AppCompatActivity context = this;
        switch (v.getId()) {
            case R.id.btnFacebook:
                provider = MobileServiceAuthenticationProvider.Facebook;
                break;
            case R.id.btnGoogle:
                provider = MobileServiceAuthenticationProvider.Google;
                break;
            case R.id.btnMicrosoft:
                provider = MobileServiceAuthenticationProvider.MicrosoftAccount;
                break;
            case R.id.btnTwitter:
                provider = MobileServiceAuthenticationProvider.Twitter;
                break;
        }

        ListenableFuture loginFuture = login.authenticate(provider);

        Futures.addCallback(loginFuture, new FutureCallback<MobileServiceUser>() {
            @Override
            public void onSuccess(MobileServiceUser user) {
                startActivity(new Intent(context, TasksActivity.class));
            }

            @Override
            public void onFailure(Throwable t) {
                if (t instanceof MobileServiceException && t.getCause().getMessage().equals("User Canceled")) {
                    return;
                }

                AlertDialog.Builder dlgAlert  = new AlertDialog.Builder(context);
                dlgAlert.setTitle("Login Failure");
                dlgAlert.setMessage("The following error occurred logging in: " + t.getMessage());
                dlgAlert.setPositiveButton("OK", null);
                dlgAlert.setCancelable(true);
                dlgAlert.create().show();
            }
        });
    }
}
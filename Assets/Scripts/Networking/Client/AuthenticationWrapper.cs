using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;

//Created by Nathan Farrer
public static class AuthenticationWrapper 
{
    public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;

    public static async Task<AuthState> DoAuth(int maxRetries = 5)
    {
        if (AuthState == AuthState.Authenticated) 
        {
            return AuthState;
        }

        if(AuthState == AuthState.Authenticating)
        {
            Debug.LogWarning("Already Authenticating!");
            await Authenticating();
            return AuthState;
        }
        await SignInAsonymouslyAsync(maxRetries);

        return AuthState;
    }

    private static async Task<AuthState> Authenticating()
    {
        while(AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }

        return AuthState;
    }
    private static async Task SignInAsonymouslyAsync(int maxRetries)
    {
        int retries = 0;
        try
        {
            AuthState = AuthState.Authenticating;
   
            while (AuthState == AuthState.Authenticating)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
            }
        }
        catch(AuthenticationException authException)
        {
            Debug.LogError(authException);
            AuthState = AuthState.Error;
        }
        catch(RequestFailedException requestException)
        {
            Debug.LogError(requestException);
            AuthState = AuthState.Error;
        }
    

        retries++;
        await Task.Delay(1000);

        if(AuthState != AuthState.Authenticated)
        {
            Debug.LogWarning($"Player was not signed in successfully after {retries} retries");
            AuthState = AuthState.TimeOut;
        }
    }
 }


public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error, 
    TimeOut
}

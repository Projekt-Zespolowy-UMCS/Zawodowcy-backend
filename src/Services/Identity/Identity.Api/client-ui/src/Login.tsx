import React from "react";
import { useForm, SubmitHandler } from "react-hook-form";
import { createUserManager } from "redux-oidc"
import {useSearchParams} from "react-router-dom";

type FormValues = {
  username: string;
  password: string;
  returnUrl: string | null
};

function Login() {
    const [searchParams, setSearchParams] = useSearchParams();
    const userManagerConfig = {
        authority: 'https://localhost:7234/',
        client_id: 'spa',
        redirect_uri: 'https://localhost:3000/',
        post_logout_redirect_uri: 'http://localhost:3000/callback',
        response_type: 'token id_token',
        scope: 'openid profile',
        loadUserInfo: true,
        monitorSession: false
    }
    
    const userManager = createUserManager(userManagerConfig)

    function signIn() {
        return userManager.signinRedirect()
    }
    
    const { register, handleSubmit } = useForm<FormValues>();
    const onSubmit: SubmitHandler<FormValues> = (data, event) => {
        // event?.preventDefault();
        // console.log(data);
        // signIn();
        // userManager.getUser().then((user) => {
        //     console.log('THEN');
        //     console.log(user)
        // }).catch(error => {
        //     console.log('ERROR');
        //     console.log(error);
        // })
        
        data.returnUrl = searchParams.get("redirect_uri")
        var xhr = new XMLHttpRequest();
        xhr.addEventListener('load', () => {
            // update the state of the component with the result here
            console.log(xhr.responseText)
            window.location.href = "https://localhost:7234";
          });
          // xhr.open('GET', 'https://localhost:7234/api/auth/login');
          // xhr.send();
            // open the request with the verb and the url
        xhr.open('POST', 'https://localhost:7234/api/auth/login')
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        // end the request
        xhr.send(JSON.stringify(data));// // s
    }
    return (
      <form onSubmit={handleSubmit(onSubmit)}>
        <p>user</p>
        <input {...register("username")} />
        <p>password</p>
        <input {...register("password")} />
        <input type="submit" />
      </form>
    );
}

export default Login;
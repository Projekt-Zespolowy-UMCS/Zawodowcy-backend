import React from "react";
import { useForm, SubmitHandler } from "react-hook-form";

type FormValues = {
  username: string;
  password: string;
};


function Login() {
    const { register, handleSubmit } = useForm<FormValues>();
    const onSubmit: SubmitHandler<FormValues> = data => {
        console.log(data);
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
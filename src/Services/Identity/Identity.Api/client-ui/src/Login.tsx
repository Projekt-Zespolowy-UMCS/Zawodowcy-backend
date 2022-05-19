import React, { FC, useReducer } from "react";
import { useSearchParams } from "react-router-dom";
import AuthService from "./services/auth-service";

type State = {
  username: string;
  password: string;
};

const initialState: State = {
  username: '',
  password: ''
}

type Action = { type: 'setUsername', payload: string }
  | { type: 'setPassword', payload: string };

const reducer = (state: State, action: Action): State => {
  switch (action.type) {
    case 'setUsername':
      return {
        ...state,
        username: action.payload
      };
    case 'setPassword':
      return {
        ...state,
        password: action.payload
      };
  }
}
export const Login: FC = () => {

  const [state, dispatch] = useReducer(reducer, initialState);
  const [searchParams, setSearchParams] = useSearchParams();
  const returnUrl = searchParams.get("ReturnUrl");
  console.log(returnUrl);

  const handleSubmit = (event: any) => {
    event.preventDefault();
    const data = {
      username: state.username,
      password: state.password,
      returnUrl: returnUrl
    }

    AuthService.login(data).then(
      response => {
        console.log("Response: ", response);
        window.location.href = response.returnUrl;
      },
      error => {
        const resMessage =
          (error.response &&
            error.response.data &&
            error.response.data.message) ||
          error.message ||
          error.toString();
        console.log(resMessage);
      }
    )

  }
  const handleUsernameChange: React.ChangeEventHandler<HTMLInputElement> =
    (event) => {
      dispatch({
        type: 'setUsername',
        payload: event.target.value
      });
    };

  const handlePasswordChange: React.ChangeEventHandler<HTMLInputElement> =
    (event) => {
      dispatch({
        type: 'setPassword',
        payload: event.target.value
      });
    }

  return (
    <>
      <form onSubmit={handleSubmit}>
        <input type="email" id="username" placeholder="Enter email" onChange={handleUsernameChange} />
        <input type="password" id="password" placeholder="enter password here" onChange={handlePasswordChange} />
        <input type="submit" />
      </form>
    </>
  )
}

export default Login;
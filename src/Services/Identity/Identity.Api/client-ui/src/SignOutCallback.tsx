import React, { FC, useEffect }  from "react";


export const SignOut:FC = () => {

    useEffect(() => {
        setTimeout(() => {
            window.location.href = "http://localhost:3000/"},
            5000)
        }, [])


    return(
        <p> You have been succesfully signed out.</p>
    );
}

export default SignOut;
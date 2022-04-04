import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { useParams, useSearchParams } from "react-router-dom";
import AuthService from './services/auth-service';
function Logout() {

    const [searchParams, setSearchParams] = useSearchParams();
    const logoutId = searchParams.get("logoutId")
    const [frame, setIframe] = useState<string | null>(null);
    let logdata: any;
    /*
    var xhr = new XMLHttpRequest();
    xhr.addEventListener('load', () => {
        // update the state of the component with the result here
        console.log(xhr.responseText)
       window.location.href = "/sign-out-callback";
    });
    const data = {
        logoutId: logoutId
    }
    xhr.open('GET', 'https://localhost:7234/api/Auth/logout')
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    // end the request
    console.log(data);
    xhr.send(JSON.stringify(data));// // s
    
    async function loggg() {
        var query = window.location.search;

        var logoutIdQuery = query && query.toLowerCase().indexOf('?logoutid=') == 0 && query;
        console.log(logoutIdQuery);
        const response = await fetch('https://localhost:7234/api/auth/logout' + logoutIdQuery, {
            credentials: 'include'
        });

        const data = await response.json();
        console.log(data)
    }
    loggg()
    */
    useEffect(() => {

        console.log("logoutId" + logoutId);
        AuthService.logout(logoutId)
            .then(response => {
                console.log("Response: ", response.postLogoutRedirectUri, response);
                setIframe(response.signOutIFrameUrl)
                window.location.href = response.postLogoutRedirectUri;
            })
            .catch(error => {
                const resMessage =
                    (error.response &&
                        error.response.data &&
                        error.response.data.message) ||
                    error.message ||
                    error.toString();
                console.log(resMessage);
            })
    }, [])

    return (
        <>
            {frame && <iframe src={frame} />}
            <div>logout??? {logoutId} AAAAA</div>
        </>
    )
}

export default Logout;

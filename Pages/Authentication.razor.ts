namespace JSInteropWithTypeScript {
    export function showPrompt(message: string) {
        return prompt(message, 'Type anything here');
    }

    export function fetchTokenAsync(origin: string, code: string, clientId: string, codeVerifier: string, redirectUri: string, authorization: string, isDebug: boolean): Promise<IAccessToken> {
        var myHeaders = new Headers();
        myHeaders.append("Host", "www.linkedin.com");
        myHeaders.append("Connection", "keep-alive");
        myHeaders.append("accept", "*/*");
        myHeaders.append("content-type", "application/x-www-form-urlencoded");
        myHeaders.append("sec-ch-ua-mobile", "?0");
        myHeaders.append("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.134 Safari/537.36 Edg/103.0.1264.77");
        myHeaders.append("sec-ch-ua-platform", "\"Windows\"");
        myHeaders.append("Origin", origin);//"https://localhost:7063"
        myHeaders.append("Sec-Fetch-Site", "cross-site");
        myHeaders.append("Sec-Fetch-Mode", "no-cors");
        myHeaders.append("Sec-Fetch-Dest", "empty");
        myHeaders.append("Authorization", authorization);
        myHeaders.append("Referer", origin);
        myHeaders.append("Accept-Encoding", "gzip, deflate, br");
        myHeaders.append("Accept-Language", "it,it-IT;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");

        var urlencoded = new URLSearchParams();
        urlencoded.append("code", code);//"AQSDB7TMO5BKICNdXYXRCmd9QTumjZE-uPRdnunmENfP2sqU7zQtfUwmhYsAK9CncbMwmju-vl-mSrtYVN7Q-88DKS06X9sYRTqPfNirCPkmtRCo75WNGyEsgnNONjL7Gr6r_GwVhcU2LIq6FYS8Y4ojVhirprlFGRm3D2jD2IxV4QwKsZX0U1oVGCs8ZjfZsj3lcipCGZ9NFFajtBg"
        urlencoded.append("grant_type", "authorization_code");
        urlencoded.append("client_id", clientId);//"78t8sa3mzru5lu"
        urlencoded.append("code_verifier", codeVerifier);//"yx2tmodaf7nx4utrz2c9gtbkecv3ctfhpyqc93ul92cxbzcia8mq8i325c9lo1gfvsaez3kdwvup7pql2hnbgmk7quu7ahpimilxc9hx1l6vzxv4f7m66x8wtej8jfjj"
        urlencoded.append("redirect_uri", redirectUri);//"https://localhost:7063/authentication/login-callback"

        var requestOptions: RequestInit = {
            method: 'POST',
            headers: myHeaders,
            body: urlencoded,
            redirect: 'follow'
        };

        return fetch("https://www.linkedin.com/oauth/v2/accessToken", requestOptions)
            .then(response => {
                // fetching the reponse body data
                if (isDebug) {
                    console.log(response);
                }

                let res = response.json() as IAccessToken;
                return res;
            })
            .catch(error => {
                console.log('error', error)
                return {
                    access_token: "",
                    expires_in: 0,
                };
            });
    }

    export interface IAccessToken {
        access_token?: string;
        expires_in?: number;
    }
}
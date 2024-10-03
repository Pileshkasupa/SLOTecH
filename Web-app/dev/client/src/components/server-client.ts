import { getCookie, setCookie } from "cookies-next";
import { io, Socket } from "socket.io-client";

declare global {
    // var socket: Socket | undefined;
    var sessions: { [Id: string]: Socket };
}

//const client = global.socket || io("http://localhost:3050");
const sessions = global.sessions || {};

export function InitSession(Id: string) {
    if (sessions[Id]) {
        return sessions[Id];
    }

    sessions[Id] = io("http://localhost:3050");

    return sessions[Id];
}

export function getSessionId(randomMath: number) {
    let sessionId = getCookie("sessionId");
    if (sessionId === undefined) {
        setCookie("sessionId", Math.random().toString(36).substring(2, 15) + randomMath.toString(36).substring(2, 15));
        sessionId = getCookie("sessionId");
    }
    return sessionId !== undefined ? sessionId.toString() : "";
}

if (process.env.NODE_ENV === "development") global.sessions = sessions;
/*
client.on("connected", (e) => {
    //console.log(e, client.id);
});

global.socket?.on("disconnected", (e) => {
    //console.log(e);
});

export default client;
*/

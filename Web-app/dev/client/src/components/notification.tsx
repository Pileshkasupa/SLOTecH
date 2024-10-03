import { useState } from "react";

enum MessageType {
    Warning = "warning",
    Error = "error",
    Info = "info",
    System = "system"
}

export default function NotificationComponent(props: { id: string; message: string; messageType: string; timeStamp: string }) {
    const [messageType, setMessageType] = useState(props.messageType.toLowerCase() as MessageType);
    const [message, setMessage] = useState(props.message);
    const [id, setId] = useState(props.id);
    const [timeStamp, setTimeStamp] = useState(props.timeStamp);

    return (
        <div className="w-full grid bg-slate-50 border rounded-md border-slate-50 p-2 grid-cols-2 mb-2">
            <div
                className={
                    "col-span-1 ml-1 rounded-md p-1 border-2 w-fit pr-2 pl-2 text-white text-sm " +
                    (messageType === MessageType.Info
                        ? "border-blue-500 bg-blue-400"
                        : messageType === MessageType.System
                        ? "border-gray-500 bg-gray-400"
                        : messageType === MessageType.Warning
                        ? "border-yellow-500 bg-yellow-400"
                        : "border-red-500 bg-red-400")
                }
            >
                {props.messageType}
            </div>
            <div className="col-span-1 text-end text-gray-400 text-sm underline mr-1 justify-end items-center flex">{id}</div>
            <div className="col-span-2 text-left leading-tight flex p-1 mt-1 items-center">
                <div>{message}</div>
            </div>
            <div className="col-span-2 text-right leading-tight flex p-1 mt-1 items-center text-gray-400 text-sm">
                <div>{timeStamp}</div>
            </div>
        </div>
    );
}

import Link from "next/link";
import { MouseEvent, useEffect, useState } from "react";
import { Socket } from "socket.io-client";
import Notification from "./notification";

export default function Header(props: { title?: string; url: string; refresh?: () => void; server?: Socket }) {
    const segments = props.url.split("/").filter(Boolean);

    const [notificationEnabled, setEnabled] = useState<boolean>(false);
    const [notifications, setNotifications] = useState([
        {
            id: "Lol",
            message: "der weiße Wolf",
            messageType: "Warning",
            timeStamp: new Date().toString()
        }
    ]);

    const getAllNotifications = () => {
        setNotifications(() => {
            const newNot = [
                {
                    id: "Lol",
                    message: "der weiße Wolf",
                    messageType: "Warning",
                    timeStamp: new Date().toString()
                },
                {
                    id: "Lamp",
                    message: "Deine Mutter, mag dich nicht so gern wie sie, den weißen Wolf mag",
                    messageType: "System",
                    timeStamp: new Date().toString()
                },
                { id: "205151", message: "Lol Test was ist hier los", messageType: "Info", timeStamp: new Date().toString() }
            ];
            return [...newNot];
        });
    };

    useEffect(() => {
        getAllNotifications();
    }, []);

    const paths = segments.map((segment, index) => {
        return {
            name: segment,
            url: "/" + segments.slice(0, index + 1).join("/")
        };
    });

    const seeNotification = () => {
        if (notificationEnabled) {
            setEnabled(false);
            return;
        }
        setEnabled(true);
    };

    return (
        <header className="bg-white text-black body-font shadow fixed w-full z-10 top-0 grid grid-cols-3">
            <div className="p-5">
                <nav className="flex flex-wrap items-center">
                    <Link className="mr-5 hover:underline transition-all text-lg" href="/">
                        Dashboard
                    </Link>
                    <span className="mr-1 border border-white select-none">/</span>
                    {paths.map((path, index) => (
                        <li key={index} className="flex items-center">
                            <Link href={path.url} className="hover:bg-slate-200 hover:border hover:rounded-md border border-white">
                                {path.name}
                            </Link>
                            {index < paths.length - 1 && <span className="mx-1 select-none">/</span>}
                        </li>
                    ))}
                </nav>
            </div>
            <div className=" flex justify-center items-center">
                <a className="text-gray-200">
                    <span className="text-xl font-extralight">{props.title ? props.title : "SLOTecH"}</span>
                </a>
            </div>
            <div className=" flex justify-end items-center mr-3 ">
                <div
                    className="p-1 pr-[0.45rem] pl-[0.45rem] mr-3 bg-slate-100 select-none cursor-pointer text-lg hover:text-2xl hover:mr-[0.7rem] transition-all border-2 border-slate-100 rounded-full hover:border-slate-300 hover:bg-slate-300"
                    onClick={seeNotification}
                >
                    🔔
                </div>{" "}
                {notificationEnabled && (
                    <div className="overflow-y-scroll absolute w-80 bg-slate-300 h-[30rem] mt-[34.5rem] border border-slate-300 p-2 shadow-lg rounded-2xl mr-[-0.5rem]">
                        {notifications.map((notification) => {
                            return (
                                <Notification
                                    key={notification.id}
                                    id={notification.id}
                                    message={notification.message}
                                    messageType={notification.messageType}
                                    timeStamp={notification.timeStamp}
                                />
                            );
                        })}
                    </div>
                )}
                {props.refresh && (
                    <div className="mr-2 cursor-pointer hover:underline text-lg" onClick={props.refresh}>
                        Refresh
                    </div>
                )}
                <Link href="/settings" className="hover:underline text-lg">
                    Settings
                </Link>
            </div>
        </header>
    );
}

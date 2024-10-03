"use client";

import Message from "@/types/Message";
import { DetailedHTMLProps, HTMLAttributes, useEffect, useState } from "react";
import { EventType } from "../page";

export default function MessageEventComp(props: {
    divAttributes: DetailedHTMLProps<HTMLAttributes<any>, HTMLElement>;
    nextFunc: (view: EventType, messages: Message[], title: string) => void;
    devices: Message[];
}) {
    const [devices, setDevices] = useState<Message[]>(props.devices);
    const [title, setTitle] = useState<string>("");

    useEffect(() => {
        setDevices(props.devices);
    }, [props.devices]);

    const [finishedMessages, setFinishedMessages] = useState<Message[]>([]);

    return (
        <div className="body" {...props.divAttributes}>
            <h1 className="pt-[3em] bg-gray-100 text-black flex justify-center text-4xl font-extrabold">Messages </h1>
            <div className=" bg-gray-100 w-screen text-black h-screen grid-cols-6 grid">
                <div className="col-span-6 bg-slate-200 border rounded-md m-5 mt-3 mb-3 flex flex-wrap content-start">
                    {devices.map((device, index) => {
                        return (
                            <div
                                className="p-2 m-1 bg-slate-300 border rounded-md  text-black h-14 text-2xl font-bold w-[17rem] flex flex-wrap justify-center items-center "
                                key={index}
                            >
                                <div className="flex flex-wrap">
                                    <div className=" select-none text-base flex justify-center items-center mr-1" title={device.Id}>
                                        Set {device.Title} Value to
                                    </div>
                                    <input
                                        type="text"
                                        className="w-10 border rounded-md text-base text-center border-none bg-slate-200 focus:outline-none focus:ring-2 focus:ring-transparent"
                                        placeholder="1"
                                        onChange={(e) => {
                                            if (e.target.value === "") {
                                                setFinishedMessages((prevMessages) => {
                                                    return prevMessages.filter((m) => m.Id !== device.Id);
                                                });
                                                return;
                                            }
                                            setFinishedMessages((prevMessages) => {
                                                if (prevMessages.find((m) => m.Id === device.Id)) {
                                                    prevMessages = prevMessages.filter((m) => m.Id !== device.Id);
                                                }
                                                return [...prevMessages, { ...device, Value: +e.target.value * 1000 }];
                                            });
                                            return;
                                        }}
                                    />
                                </div>
                            </div>
                        );
                    })}
                </div>
                <div className="flex col-span-6 w-full">
                    <div className="flex content-center items-center h-14 justify-start">
                        <h1 className=" bg-gray-100 text-black flex text-2xl ml-5 mr-2 font-extrabold whitespace-nowrap">Event Title: </h1>
                        <input
                            type="text"
                            className="h-14 focus:outline-none focus:ring-2 focus:ring-transparent bg-transparent mr-2 text-2xl"
                            placeholder="Title"
                            defaultValue={title}
                            onChange={(e) => setTitle(e.target.value)}
                        />
                    </div>
                    <div className="flex justify-end w-full">
                        <div
                            className="p-2 flex items-center bg-blue-400 border rounded-md mr-5 text-black h-14 text-center text-2xl font-bold hover:bg-blue-600  cursor-pointer hover:text-white transition-all"
                            onClick={() => props.nextFunc(EventType.Finish, finishedMessages, title)}
                        >
                            Finish
                        </div>{" "}
                    </div>
                </div>
            </div>
        </div>
    );
}

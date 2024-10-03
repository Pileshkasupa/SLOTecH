"use client";

import { getAllVisibleComponents } from "@/components/components";
import Message from "@/types/Message";
import Image from "next/image";
import { DetailedHTMLProps, HTMLAttributes, useEffect, useState } from "react";
import { EventType } from "../page";

export default function ConditionSelection(props: {
    divAttributes: DetailedHTMLProps<HTMLAttributes<any>, HTMLElement>;
    nextFunc: (view: EventType) => void;
    addOrRemoveDevice: (device: Message) => void;
}) {
    const [devices, setDevices] = useState<Message[]>([]);
    const [selectedDevices, setSelectedDevices] = useState<string[]>([]);

    useEffect(() => {
        getAllVisibleComponents().then((components) => {
            setDevices(components);
        });
    }, []);

    return (
        <div className="body" {...props.divAttributes}>
            <h1 className="pt-[3em] bg-gray-100 text-black flex justify-center text-4xl font-extrabold">Condition Selection</h1>
            <div className="bg-gray-100 w-screen text-black h-screen grid-cols-6 grid">
                <div className="col-span-6 bg-slate-200 border rounded-md m-5 mt-3 mb-3 flex flex-wrap justify-start content-start">
                    {devices.map((device) => {
                        return (
                            <div
                                key={device.Id}
                                className={"flex w-fit h-fit m-2 mr-0 cursor-pointer select-none"}
                                title={device.Id}
                                onClick={() => {
                                    setSelectedDevices((prevDevices) => {
                                        if (prevDevices?.includes(device.Id)) {
                                            return prevDevices.filter((d) => d !== device.Id);
                                        } else {
                                            return [...prevDevices, device.Id];
                                        }
                                    });
                                    props.addOrRemoveDevice(device);
                                }}
                            >
                                <div
                                    className={
                                        " grid grid-rows-2 h-24  justify-center rounded-xl w-24 border-2 transition-all " +
                                        (selectedDevices.includes(device.Id) ? "border-blue-400 bg-blue-200" : "bg-slate-300")
                                    }
                                >
                                    <Image
                                        src={device.Image ? device.Image : ""}
                                        alt={device.Image ? device.Image : ""}
                                        width={55}
                                        height={55}
                                        className={"block m-auto "}
                                    />
                                    <div className="p-2 text-lg">{device.Title}</div>
                                </div>
                            </div>
                        );
                    })}
                </div>
                <div
                    className="p-2 flex items-center justify-center bg-blue-400 border rounded-md mr-5 text-black h-14 text-center text-2xl font-bold hover:bg-blue-600 col-span-1 col-start-6 cursor-pointer hover:text-white transition-all"
                    onClick={() => props.nextFunc(EventType.Condition)}
                >
                    Next
                </div>
            </div>
        </div>
    );
}

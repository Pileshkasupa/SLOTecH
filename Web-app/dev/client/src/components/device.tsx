"use client";

import Image from "next/image";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { getAllNonDeclaredComponents, updateComponentById } from "./components";
import { useEffect, useState } from "react";
import Message from "@/types/Message";
import { Socket } from "socket.io-client";

export default function Device(props: Message & { server: Socket }) {
    const router = useRouter();
    const [message, setMessage] = useState<Message>(props);
    const [value, setValue] = useState(props.Value);

    useEffect(() => {
        /*server.on("hello", (arg) => {
            console.log("hello", arg);
            setHello(true);
        });
        */

        props.server.on("update", (arg: Message[]) => {
            arg.forEach(async (updatedMessage: Message) => {
                if (message.Id === updatedMessage.Id) {
                    const oldMessage = message;
                    const newMessage = {
                        Id: updatedMessage.Id,
                        MessageType: updatedMessage.MessageType,
                        Room: updatedMessage.Room,
                        Type: updatedMessage.Type,
                        Unit: updatedMessage.Unit,
                        Value: updatedMessage.Value,
                        Image: oldMessage.Image,
                        TimeStamp: oldMessage.TimeStamp,
                        Title: oldMessage.Title,
                        Tags: oldMessage.Tags
                    } as Message;
                    setMessage(newMessage);
                    // Replace the existing device
                    setValue(newMessage.Value);
                }
            });
        });
    }, [props.server]);

    const clickIcon = () => {
        switch (message.Type) {
            case "lamp":
                console.log(value);
                const status = updateComponentById({
                    Id: message.Id,
                    Room: message.Room,
                    Value: value === 0 ? 1 : 0,
                    Unit: message.Unit,
                    Type: message.Type,
                    MessageType: "Communication"
                } as Message).then((data) => {
                    setValue((prevValue) => (prevValue === 0 ? 1 : 0));
                });

                break;

            default:
                clickDevice();
                break;
        }
    };
    const clickDevice = () => {
        router.push(`/devices/${props.Id}`);
    };
    return (
        <div className="w-[12.5em] h-[12.5em] border-[2px] rounded-lg border-white bg-white m-2 text-black grid grid-cols-2 grid-rows-2 select-none">
            <div
                className={
                    "rounded-full bg-gray-100 w-[5rem] h-[5rem] border border-white m-3 flex justify-center " +
                    `${
                        message.Type === "lamp"
                            ? " hover:bg-slate-300 transition-all hover:w-[5.5rem] hover:h-[5.5rem] hover:m-2 hover:cursor-pointer"
                            : ""
                    }`
                }
                onClick={clickIcon}
            >
                <Image src={message.Image ? message.Image : "/lamp.png"} alt={message.Id} width={55} height={55} className={"block m-auto "} />
            </div>
            <div className="flex items-center h-full w-full ml-1 select-none" onClick={clickDevice}>
                <h1 className="text-lg font-bold break-words underline w-full select-none">{message.Title}</h1>
            </div>
            <div className="flex h-full flex-col justify-end" onClick={clickDevice}>
                <div className="text-left bg-gray-100 border border-white rounded-lg w-min p-2 font-bold m-3">
                    <h1>{value}</h1>
                </div>
            </div>
            <div className="flex h-full w-full justify-end items-end" onClick={clickDevice}>
                <h1 className="text-gray-300 hover:underline m-3 p-2 bg-white border text-right text-xs border-white rounded-lg">{message.Id}</h1>
            </div>
        </div>
    );
}

export function AddDevice() {
    const [isVisible, setIsVisible] = useState(false);
    useEffect(() => {
        getAllNonDeclaredComponents().then((data) => {
            data.length >= 1 ? setIsVisible(true) : setIsVisible(false);
        });
    }, []);
    return isVisible ? (
        <div className="w-[12.5em] h-[12.5em] border-[2px] rounded-lg border-none bg-gray-100 m-2 text-black grid grid-cols-1 grid-rows-1 select-none items-center place-items-center justify-center overflow-hidden cursor-default hover:bg-gray-50 transition-all">
            {" "}
            <Link href="/settings/new_device">
                <div className="rounded-full bg-gray-200 w-[6rem] h-[6rem] border border-none m-3 flex justify-center items-center hover:cursor-pointer hover:bg-slate-300 hover:h-[20.5rem] hover:w-[20.5rem] transition-all text-2xl hover:text-3xl">
                    <span className="">Add</span>
                </div>{" "}
            </Link>
        </div>
    ) : (
        <></>
    );
}

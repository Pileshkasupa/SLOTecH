"use client";
import Alert from "@/components/alert";
import { getComponentById, getHistoryOfComponentById, removeComponentById, updateComponentById } from "@/components/components";
import Header from "@/components/header";
import Message from "@/types/Message";
import { usePathname, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { InitSession, getSessionId } from "@/components/server-client";

export default function Page({ params }: { params: { device: string } }) {
    let sessionId = getSessionId(Math.random());
    const client = InitSession(sessionId);

    const id = params.device;
    const [title, setTitle] = useState<string>();
    const [room, setRoom] = useState("");
    const [value, setValue] = useState(0);
    const [type, setType] = useState("");
    const [unit, setUnit] = useState("");
    const [alert, setAlert] = useState(false);
    const [history, setHistory] = useState<Message[]>([]);
    const [messageType, setMessageType] = useState("");
    const [tags, setTags] = useState<string[]>([]);
    const router = useRouter();

    useEffect(() => {
        client.emit("client-ready");
    }, []);

    useEffect(() => {
        client.on("update", (arg: Message[]) => {
            arg.forEach(async (updatedMessage: Message) => {
                if (id === updatedMessage.Id) {
                    // Replace the existing device
                    setValue(updatedMessage.Value);
                    setMessageType(updatedMessage.MessageType);
                    setRoom(updatedMessage.Room);
                    setType(updatedMessage.Type);
                    setUnit(updatedMessage.Unit);
                }
            });
        });
    }, [client]);

    useEffect(() => {
        getComponentById(id).then((data: Message) => {
            if (data.Title) setTitle(data.Title);
            setRoom(data.Room);
            setValue(data.Value);
            setType(data.Type);
            setUnit(data.Unit);
            setMessageType(data.MessageType);
            if (data.Tags) setTags(data.Tags);
        });

        getHistoryOfComponentById(id).then((data) => {
            setHistory(data);
        });
    }, []);

    const removeDevice = async () => {
        const res = await removeComponentById(id);
        // const res = 200;
        if (res === 200) {
            setAlert(true);
        } else {
            setAlert(false);
        }
    };
    const changeValue = async () => {
        const res = await updateComponentById({
            Id: id,
            Room: room,
            Value: value === 0 ? 1 : 0,
            Unit: unit,
            Type: type,
            MessageType: messageType
        });
        if (res === 200) {
            setValue((prevValue) => (prevValue === 0 ? 1 : 0));
        }
    };

    return (
        <div className="h-screen w-screen flex flex-wrap bg-gray-200">
            <Header url={usePathname()} server={client} />
            <Alert
                message={
                    <p>
                        {title}
                        <span className="text-slate-300 text-sm">({id})</span> was removed!
                    </p>
                }
                trigger={alert}
                closingFunction={() => {
                    setAlert(false);
                    router.push("/devices");
                }}
                automaticClosingTimeInMs={5000}
            />
            <div className="pt-16 text-black flex flex-col items-center justify-center h-screen w-full">
                <h1 className="text-6xl font-bold mb-2 hidden md:flex">{title ? title : "Title"}</h1>
                <div className="w-[70%] h-[70%] min-h-[300px] bg-white m-10 mt-1 border rounded-3xl transition-all">
                    <div className="grid grid-cols-1 grid-rows-2 h-full">
                        <div className="flex flex-col p-5 h-full w-full">
                            <h1 className="text-4xl font-bold mb-2 self-center sm:flex md:hidden">{title ? title : "Title"}</h1>
                            <div className="flex w-full justify-between">
                                <h1 className="text-lg font-light mb-2 mt-1 text-gray-300 select-none cursor-default">
                                    Id: <span className="underline"> {id}</span>
                                </h1>
                                <div
                                    className="items-end justify-end border-2 rounded-md p-1 m-1 bg-slate-200 border-slate-200 hover:border-slate-300 hover:bg-slate-300 cursor-pointer select-none transition-all hover:p-2 hover:m-0"
                                    onClick={removeDevice}
                                >
                                    Remove
                                </div>
                            </div>
                            <h1 className="text-xl mb-2">Room: {room}</h1>
                            <h1 className="text-xl mb-2">
                                Value: {value} ({unit})
                            </h1>
                            <h1 className="text-xl mb-2">Type: {type}</h1>
                            <h1 className="text-xl mb-2">
                                Tags: {tags.join(", ")}
                                {tags.length === 0 && <span>No Tags defined</span>}
                            </h1>
                            {type === "lamp" && (
                                <div
                                    className="text-xl mb-2 border rounded-md p-2 m-1 w-fit bg-slate-200 select-none cursor-pointer hover:bg-slate-300 transition-all hover:p-3 hover:m-0"
                                    onClick={changeValue}
                                >
                                    {value === 1 ? "Turn Off" : "Turn On"}
                                </div>
                            )}
                        </div>
                        <div className="p-5 h-full w-full flex flex-col justify-end">
                            <h1 className="text-xl mb-2 text-black select-none">History: </h1>
                            <div className="border-4 border-solid h-64 rounded-xl overflow-y-scroll">
                                {history.map((message) => {
                                    return (
                                        <div key={message.TimeStamp + Math.random().toString()}>
                                            {message.TimeStamp} - {message.Value}
                                        </div>
                                    );
                                })}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

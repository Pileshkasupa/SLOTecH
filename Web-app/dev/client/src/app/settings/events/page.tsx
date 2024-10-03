"use client";
import Header from "@/components/header";
import { InitSession, getSessionId } from "@/components/server-client";
import { usePathname } from "next/navigation";
import ConditionSelection from "./components/conditionSelection";
import { useEffect, useState } from "react";
import Message from "@/types/Message";
import Condition, { ConditionType } from "./components/condition";
import MessageSelection from "./components/MessageSelection";
import MessageEventComp from "./components/message";
import FinishedEventComp from "./components/finishedEventComp";
import { URL } from "@/components/consts";

type event = {
    Title: string;
    Conditions: ConditionType[];
    Messages: Message[];
};

//INFO: Name:Condition&Condition&Condition&...%Message&Message&Message
export default function Home() {
    let sessionId = getSessionId(Math.random());
    const client = InitSession(sessionId);
    const pathname = usePathname();
    const [view, setView] = useState<EventType>(EventType.Finish);
    const [events, setEvents] = useState<event[]>([
        /*{
            Title: "Test",
            Conditions: [{ Id: "lamp" , Operator: ">", Value: "5" }] as ConditionType[],
            Messages: [
                { Id: "Lamp", MessageType: "action", Room: "Kitchen", Type: "lamp", Unit: "boolean", Value: 1, Title: "Lamp" },
                { Id: "Nanoleaf", MessageType: "action", Room: "Kitchen", Type: "lamp", Unit: "boolean", Value: 1, Title: "Nanoleaf" },
                { Id: "3", MessageType: "sensor", Room: "LivingRoom", Type: "temperature", Unit: "celsius", Value: 23, Title: "Temperature" },
                { Id: "4", MessageType: "sensor", Room: "My Room", Type: "temperature", Unit: "celsius", Value: 50, Title: "Lol" }
            ]
        },
        { Title: "Test2", Conditions: [], Messages: [] }
    */
    ]);

    useEffect(() => {
        fetch(`${URL}/api/getScenes`).then((res) => {
            res.json().then((data: any) => {
                setEvents(data);
            });
        });
    }, []);

    const [conditions, setConditions] = useState<Message[]>([]);
    const [finishedConditions, setFinishedConditions] = useState<ConditionType[]>([]);

    const addOrRemoveConditionDevice = (device: Message) => {
        setConditions((prevDevices) => {
            if (prevDevices?.includes(device)) {
                return prevDevices.filter((d) => d !== device);
            } else {
                return [...prevDevices, device];
            }
        });
    };

    const [messages, setMessages] = useState<Message[]>([]);
    const addOrRemoveMessageDevice = (device: Message) => {
        setMessages((prevDevices) => {
            if (prevDevices?.includes(device)) {
                return prevDevices.filter((d) => d !== device);
            } else {
                return [...prevDevices, device];
            }
        });
    };

    const finishEvent = (title: string, conditions: ConditionType[], messages: Message[]) => {
        const event = {
            Title: title,
            Conditions: conditions,
            Messages: messages
        };

        setEvents((prevEvents) => {
            return [...prevEvents, event];
        });

        let conditionsAsString = conditions.map((c) => `${c.Id}${c.Operator}${c.Value}`).join("&");
        let messagesAsString = messages.map((m) => `${m.Id};${m.Room};${m.Value};${m.Unit};${m.Type}`).join("&");
        decodeURI;
        const url = `${URL}/api/createScene?scene=${event.Title}:${conditionsAsString}?${messagesAsString}`;
        console.log(url);

        fetch(url, {
            method: "POST",
            headers: { "Content-Type": "text/plain" }
        });
    };

    const toggleView = (view: EventType) => {
        setView(view);
        console.log(view);
    };

    return (
        <div className="body">
            <Header url={pathname} server={client} />
            <ConditionSelection
                nextFunc={toggleView}
                divAttributes={{ className: view === EventType.ConditionSelection ? "" : "hidden" }}
                addOrRemoveDevice={addOrRemoveConditionDevice}
            />
            <Condition
                devices={conditions}
                nextFunc={(v, c) => {
                    setFinishedConditions(c);
                    toggleView(v);
                }}
                divAttributes={{ className: view === EventType.Condition ? "" : "hidden" }}
            />
            <MessageSelection
                divAttributes={{ className: view === EventType.MessageSelection ? "" : "hidden" }}
                nextFunc={toggleView}
                addOrRemoveDevice={addOrRemoveMessageDevice}
            />
            <MessageEventComp
                divAttributes={{ className: view === EventType.Message ? "" : "hidden" }}
                devices={messages}
                nextFunc={(v, m, t) => {
                    finishEvent(t, finishedConditions, m);
                    toggleView(v);
                }}
            />
            <div className={(view === EventType.Finish ? "" : "hidden ") + "h-screen bg-slate-100 text-black"}>
                <div className="h-[13rem] ">
                    <h1 className="pt-[3em] bg-gray-100 text-black flex justify-center text-4xl font-extrabold pb-5 w-full">Events</h1>
                    <div className="flex justify-end ">
                        <div
                            className=" select-none p-5 items-center justify-center flex bg-blue-400 border rounded-md mr-5 text-black h-14 w-fit text-center text-2xl font-bold hover:bg-blue-600 cursor-pointer hover:text-white transition-all"
                            onClick={() => toggleView(EventType.ConditionSelection)}
                        >
                            New
                        </div>
                    </div>
                </div>

                <div className="w-full bg-slate-100 h-2/3 p-5">
                    <div className="bg-slate-200 border border-slate-200 rounded-md p-2 h-full w-full flex flex-wrap content-start">
                        {events.map((event) => {
                            return (
                                <FinishedEventComp
                                    title={event.Title}
                                    conditions={event.Conditions}
                                    messages={event.Messages}
                                    key={event.Title}
                                    onDelete={(t) => {
                                        fetch(`${URL}/api/deleteScene?title=${t}`, {
                                            method: "DELETE",
                                            headers: { "Content-Type": "text/plain" }
                                        });
                                        setEvents((prevEvents) => {
                                            return prevEvents.filter((e) => e.Title !== t);
                                        });
                                    }}
                                />
                            );
                        })}
                    </div>
                </div>
            </div>
        </div>
    );
}

export enum EventType {
    ConditionSelection = "ConditionSelection",
    Condition = "Condition",
    Message = "Message",
    MessageSelection = "MessageSelection",
    Finish = "Finish"
}

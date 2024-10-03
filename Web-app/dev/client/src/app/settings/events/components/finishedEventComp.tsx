"use client";

import Message from "@/types/Message";
import { ConditionType } from "./condition";

export default function FinishedEventComp(props: { title: string; conditions: ConditionType[]; messages: Message[]; onDelete: (t: string) => void }) {
    return (
        <div className="w-full bg-slate-300 m-1 p-2 border rounded-md border-slate-300 grid grid-cols-4">
            <div className="font-bold text-xl flex text-center justify-center content-center items-center">{props.title}</div>
            <div className="w-[99%] border rounded-md border-slate-200 bg-slate-200 mr-2">
                Conditions:
                {props.conditions.map((condition, index) => (
                    <div key={index} className="flex">
                        <div className="border-4 border-l-0 border-b-0 border-r-0 border-slate-300 bg-slate-200  w-full cursor-default">
                            {condition.Id} {condition.Operator} {condition.Value}
                        </div>
                    </div>
                ))}
            </div>
            <div className="w-[99%] border rounded-md border-slate-200 bg-slate-200">
                Messages:
                {props.messages.map((message, index) => (
                    <div key={index} className="flex">
                        <div
                            className=" border-4 border-l-0 border-b-0 border-r-0 border-slate-300 bg-slate-200  w-full cursor-default"
                            title={message.Id}
                        >
                            {message.Id}: {message.Value}
                        </div>
                    </div>
                ))}
            </div>
            <div className="flex w-full justify-end items-center select-none">
                <div
                    className="flex p-2 font-bold text-lg rounded-md h-12 items-center border-4 bg-slate-200 mr-2 transition-all hover:bg-red-600 hover:text-white hover:border-red-600 cursor-pointer"
                    onClick={() => props.onDelete(props.title)}
                >
                    Delete
                </div>
            </div>
        </div>
    );
}

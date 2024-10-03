"use client";

import Message from "@/types/Message";
import { DetailedHTMLProps, HTMLAttributes, useEffect, useState } from "react";
import { EventType } from "../page";
import SymbolInput from "./symbolInput";

export type ConditionType = {
    Id: string;
    Operator: string;
    Value: number;
};

export default function Condition(props: {
    divAttributes: DetailedHTMLProps<HTMLAttributes<any>, HTMLElement>;
    nextFunc: (view: EventType, conditions: ConditionType[]) => void;
    devices: Message[];
}) {
    const [devices, setDevices] = useState<Message[]>(props.devices);

    useEffect(() => {
        setDevices(props.devices);
    }, [props.devices]);

    const [conditions, setConditions] = useState<ConditionType[]>([]);
    const [operations, setOperations] = useState<{ [id: string]: string }>({});
    const [values, setValues] = useState<{ [id: string]: number }>({});

    return (
        <div className="body" {...props.divAttributes}>
            <h1 className="pt-[3em] bg-gray-100 text-black flex justify-center text-4xl font-extrabold">Condition </h1>
            <div className=" bg-gray-100 w-screen text-black h-screen grid-cols-6 grid">
                <div className="col-span-6 bg-slate-200 border rounded-md m-5 mt-3 mb-3 flex flex-wrap content-start">
                    {devices.map((device, index) => {
                        return (
                            <div
                                className="p-2 m-1 bg-slate-300 border rounded-md  text-black h-14 text-2xl font-bold w-[17rem] flex flex-wrap justify-center items-center "
                                key={index}
                            >
                                <div className="flex flex-wrap">
                                    <div className=" select-none text-base flex justify-center items-center" title={device.Id}>
                                        {device.Title}
                                    </div>
                                    <SymbolInput
                                        onChange={(e) => {
                                            const allowedChars = ["<", ">", "="];

                                            // Check if the new input is one of the allowed characters and if it's a single character
                                            if (allowedChars.includes(e.target.value) && e.target.value.length === 1) {
                                                setOperations({ ...operations, [device.Id]: e.target.value });
                                            } else if (e.target.value === "") {
                                                setOperations({ ...operations, [device.Id]: "" });
                                                return;
                                            }

                                            return;
                                        }}
                                    />

                                    <input
                                        type="text"
                                        className="w-10 border rounded-md text-base text-center"
                                        placeholder="5"
                                        defaultValue={values[device.Id]}
                                        onChange={(e) => {
                                            if (isNaN(e.target.value)) {
                                                setValues({ ...values, [device.Id]: -9999 });
                                                return;
                                            }
                                            setValues({ ...values, [device.Id]: +e.target.value * 1000 });
                                            return;
                                        }}
                                    />
                                    <div
                                        className={
                                            "text-base ml-3 cursor-pointer  border rounded-md p-1 transition-all  select-none " +
                                            (conditions.find((c) => c.Id === device.Id)
                                                ? "bg-blue-600 border-blue-600 text-white hover:bg-red-600 hover:border-red-600 hover:text-white "
                                                : "bg-blue-400 border-blue-400 hover:bg-blue-600 hover:border-blue-600 hover:text-white")
                                        }
                                        onClick={() => {
                                            setConditions((prevConditions) => {
                                                if (!operations[device.Id] || !values[device.Id]) {
                                                    return prevConditions;
                                                }
                                                if (prevConditions.find((c) => c.Id === device.Id)) {
                                                    prevConditions = prevConditions.filter((c) => c.Id !== device.Id);
                                                    return prevConditions;
                                                }
                                                return [
                                                    ...prevConditions,
                                                    { Id: device.Id, Operator: operations[device.Id], Value: values[device.Id] }
                                                ];
                                            });
                                        }}
                                    >
                                        {conditions.find((c) => c.Id === device.Id) ? "Added" : "Add"}
                                    </div>
                                </div>
                            </div>
                        );
                    })}
                </div>
                <div
                    className="p-2 flex items-center justify-center bg-blue-400 border rounded-md mr-5 text-black h-14 text-center text-2xl font-bold hover:bg-blue-600 col-span-1 col-start-6 cursor-pointer hover:text-white transition-all"
                    onClick={() => props.nextFunc(EventType.MessageSelection, conditions)}
                >
                    Next
                </div>
            </div>
        </div>
    );
}

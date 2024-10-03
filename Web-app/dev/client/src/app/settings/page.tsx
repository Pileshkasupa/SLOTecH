"use client";
import { getAllNonDeclaredComponents } from "@/components/components";
import Header from "@/components/header";
import { redirect, usePathname, useRouter } from "next/navigation";
import { useState } from "react";

export default function Home() {
    const router = useRouter();
    const [addDeviceIsVisible, setAddDeviceIsVisible] = useState(false);

    getAllNonDeclaredComponents().then((data) => (data.length >= 1 ? setAddDeviceIsVisible(true) : setAddDeviceIsVisible(false)));

    return (
        <div className="h-screen w-screen flex flex-wrap bg-gray-100">
            <Header url={usePathname()} />
            <div className="pt-[5rem] text-black flex flex-col h-full w-full items-center">
                {addDeviceIsVisible && (
                    <input
                        type="button"
                        value="Add Device"
                        className="border-2 rounded-md border-slate-200 p-2 bg-slate-200 hover:bg-slate-300 hover:cursor-pointer hover:border-slate-300 transition-all hover:p-3 m-2 hover:m-1"
                        onClick={() => router.push("settings/new_device")}
                    />
                )}
                <input
                    type="button"
                    value="Add Events"
                    className="border-2 rounded-md border-slate-200 p-2 bg-slate-200 hover:bg-slate-300 hover:cursor-pointer hover:border-slate-300 transition-all hover:p-3 m-2 hover:m-1"
                    onClick={() => router.push("settings/events")}
                />
                ...more settings soon to be implemented!
            </div>
        </div>
    );
}

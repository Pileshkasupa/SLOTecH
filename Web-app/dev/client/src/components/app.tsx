"use client";
import Header from "@/components/header";
import Device, { AddDevice } from "@/components/device";
import { usePathname } from "next/navigation";
import { getAllVisibleComponents } from "./components";
import { useEffect, useState } from "react";
import Message from "@/types/Message";
import { InitSession, getSessionId } from "./server-client";

export default function Home() {
    let sessionId = getSessionId(Math.random());
    const client = InitSession(sessionId);

    const [devices, setDevices] = useState<Message[]>([]);
    const [tags, setTags] = useState<string[]>([]);
    const [displayedDevices, setDisplayedDevices] = useState<Message[]>(devices);
    const pathname = usePathname();
    const [newTag, setNewTag] = useState("");
    const [isOpen, setIsOpen] = useState(false);
    const [rooms, setRooms] = useState<string[]>([]);
    const [selectedRooms, setSelectedRooms] = useState<string[]>([]);

    useEffect(() => {
        client.emit("client-ready");
    }, []);

    const loadData = async () => {
        setDevices([]);
        const deviceList = await getAllVisibleComponents();
        deviceList.forEach((newDevice) => {
            setDevices((prevDevices) => {
                const existingDeviceIndex = prevDevices.findIndex((d) => d.Id === newDevice.Id);
                if (existingDeviceIndex !== -1) {
                    // Replace the existing device
                    return [...prevDevices.slice(0, existingDeviceIndex), newDevice, ...prevDevices.slice(existingDeviceIndex + 1)];
                } else {
                    // Add new device
                    return [...prevDevices, newDevice];
                }
            });

            setRooms((prevRooms) => {
                if (prevRooms.includes(newDevice.Room)) return prevRooms;
                return [...prevRooms, newDevice.Room];
            });
        });
        setDisplayedDevices(deviceList);
    };
    useEffect(() => {
        loadData();
    }, []);

    useEffect(() => {
        if (tags.length === 0 && selectedRooms.length === 0) {
            setDisplayedDevices(devices);
            return;
        }
        setDisplayedDevices(
            devices.filter((device) => {
                if (selectedRooms.length > 0 && !selectedRooms.includes(device.Room)) return false;

                let allTags = true;
                tags.forEach((tag) => {
                    if (!device.Tags?.includes(tag)) allTags = false;
                });

                return allTags;
            })
        );
    }, [tags, selectedRooms]);

    return (
        <div className="body">
            <Header url={pathname} refresh={loadData} server={client} />
            <div className="pt-[5em] bg-slate-200 w-full text-black pl-2 select-none flex flex-wrap justify-start content-start">
                <div className="w-1/3 flex flex-wrap bg-slate-200">
                    <input
                        type="text"
                        value={newTag}
                        placeholder="Add Tag"
                        className="bg-slate-100 text-black text-xl w-[15rem] font-light focus:outline-none rounded-md pl-2 p-1 "
                        onKeyDown={(e: any) => {
                            if (e.key === "Enter") {
                                if (tags.includes(e.target.value)) {
                                    return;
                                }
                                setTags((prevTags) => [...prevTags, newTag]);
                                setNewTag("");
                            }
                        }}
                        onChange={(e: any) => {
                            setNewTag(e.target.value);
                        }}
                    />
                    <div className="relative w-[5rem] ">
                        <button
                            onClick={() => {
                                setIsOpen(!isOpen);
                            }}
                            className="px-4 py-2 ml-2 text-white bg-slate-500 hover:bg-slate-700 focus:outline-none rounded-md transition-all"
                        >
                            Rooms
                        </button>

                        {isOpen && (
                            <div className="absolute left-0 w-40 mt-0 py-2 bg-white border rounded shadow-xl">
                                {rooms.map((room) => (
                                    <div key={room}>
                                        <input
                                            type="checkbox"
                                            id={room}
                                            name={room}
                                            defaultChecked={selectedRooms.includes(room)}
                                            className="px-4 py-2 mr-1 ml-1 hover:bg-gray-100 rounded cursor-pointer"
                                            onChange={(e: any) => {
                                                if (e.target.checked) {
                                                    if (selectedRooms.includes(room)) return;
                                                    setSelectedRooms((prevRooms) => [...prevRooms, room]);
                                                } else {
                                                    setSelectedRooms((prevRooms) => prevRooms.filter((r) => r !== room));
                                                }
                                            }}
                                        />
                                        <label htmlFor={room}>{room}</label>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                </div>

                <div className="flex flex-wrap text-black border-slate-100 border-2 p-1 bg-slate-100 rounded-md w-full text-xl focus:outline-none mt-2">
                    {tags.map((tag) => (
                        <div
                            className="bg-slate-100 mr-1 p-1 rounded-md  cursor-pointer hover:bg-slate-300 transition-all"
                            key={tag}
                            onClick={() => {
                                setTags((prevTags) => prevTags.filter((t) => t !== tag));
                            }}
                        >
                            {tag}
                        </div>
                    ))}
                    {tags.length === 0 && <div className="text-gray-400 p-1">No Tags</div>}
                </div>
            </div>
            <div className="h-screen w-screen flex flex-wrap bg-gray-200 justify-start content-start">
                {displayedDevices.map((device) => {
                    return <Device key={device.Id} {...device} server={client} />;
                })}
                <AddDevice />
            </div>
        </div>
    );
}

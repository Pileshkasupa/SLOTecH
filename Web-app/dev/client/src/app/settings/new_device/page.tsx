"use client";
import { addComponent, getAllNonDeclaredComponents, loadImages } from "@/components/components";
import Header from "@/components/header";
import Message from "@/types/Message";
import Image from "next/image";
import { usePathname, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Home() {
    const [text, setText] = useState("Title");
    const [id, setId] = useState("");
    const [another, setAnother] = useState(false);
    const router = useRouter();
    const [devices, setDevices] = useState<Message[]>([]);
    const [images, setImages] = useState<string[]>([]);
    const [image, setImage] = useState("/lamp.png");
    const [tags, setTags] = useState<string[]>([]);
    const [newTag, setNewTag] = useState("");
    const handleChange = (event: any) => {
        setText(event.target.value);
    };

    const setFocus = (e: any) => {
        setId(e.target.innerText);
        setText(e.target.innerText);
    };
    const addNewComponent = async (e: any) => {
        if (id === "") {
            return;
        }

        const newComponent = await addComponent({ Id: id, Title: text, Image: image, Tags: tags });

        another && devices.length > 1 ? loadData() : router.push(`/devices/`);
        setId("");
        setText("Title");
    };
    const loadData = async () => {
        setDevices([]);
        const deviceList = await getAllNonDeclaredComponents();
        deviceList.forEach(async (newDevice) => {
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
        });
    };
    useEffect(() => {
        loadData();
        loadImages().then((images) => {
            setImages(images);
        });
    }, []);
    return (
        <div className="w-screen h-screen bg-gray-100">
            <Header url={usePathname()} />
            <div className="h-full w-full flex flex-col items-center justify-center pt-[5rem]">
                <div className=" m-2 flex  justify-center">
                    <input
                        type="text"
                        value={text}
                        onChange={handleChange}
                        className="p-2 bg-transparent focus:outline-none focus:ring-2 focus:ring-transparent text-black h-14 text-center text-6xl font-bold  w-full"
                    />
                </div>
                <div className="flex h-1/5 w-1/4 items-center">
                    <ul className="text-black flex border rounded-md h-full w-full bg-slate-200 ">
                        {devices.map((device) => {
                            return (
                                <li
                                    key={device.Id}
                                    className={
                                        "transition-all border rounded-md bg-slate-100 border-slate-200 p-2 w-full m-1 cursor-pointer flex items-center justify-center hover:bg-slate-300 hover:border-slate-300 focus:bg-slate-300 " +
                                        `${device.Id === id ? "bg-slate-300 border-slate-300" : ""}}`
                                    }
                                    onClick={setFocus}
                                >
                                    {device.Id}
                                </li>
                            );
                        })}
                    </ul>
                </div>
                <div className="text-black flex border rounded-md bg-slate-200 w-1/4 justify-center mt-2">
                    {images.map((imagePath) => (
                        <Image
                            src={imagePath}
                            key={imagePath}
                            width={65}
                            height={65}
                            alt={`${imagePath}`}
                            className={
                                "m-1 bg-slate-100 rounded-md border cursor-pointer hover:border-slate-300 hover:bg-slate-300 transition-all select-none" +
                                `${imagePath === image ? " border-slate-300 bg-slate-300" : ""}`
                            }
                            onClick={() => {
                                setImage(imagePath);
                            }}
                        />
                    ))}
                </div>
                <div className="text-black flex border rounded-md bg-slate-200 w-1/4 justify-center mt-2">
                    <input
                        type="text"
                        value={newTag}
                        placeholder="Add a new tag"
                        onChange={(e: any) => {
                            setNewTag(e.target.value);
                        }}
                        onKeyDown={(e: any) => {
                            if (e.key === "Enter") {
                                if (!tags.includes(newTag)) {
                                    setTags((prevTags) => [...prevTags, newTag]);
                                    setNewTag("");
                                }
                                console.log(tags);
                            }
                        }}
                        className="p-2 bg-slate-100 focus:outline-none m-1 focus:ring-2 focus:ring-transparent text-black h-8 text-center text-xl font-bold  w-full"
                    />
                </div>{" "}
                <div className="text-black flex border-slate-300 border-4 rounded-md bg-slate-200 w-1/4 justify-center mt-2 h-12">
                    {tags.map((tag) => (
                        <div
                            key={tag}
                            className="text-black flex items-center justify-center bg-slate-100 rounded-md border border-slate-200 p-2 m-1 cursor-pointer hover:bg-slate-300 hover:border-slate-300 transition-all select-none"
                            onClick={() => {
                                setTags((prevTags) => prevTags.filter((t) => t !== tag));
                            }}
                        >
                            {tag}
                        </div>
                    ))}
                    {tags.length === 0 && (
                        <div className="text-black flex items-center italic justify-center rounded-md border border-slate-200 p-2 m-1  select-none">
                            No Tags Added
                        </div>
                    )}
                </div>
                <div className=" w-1/4">
                    <button
                        className="p-2 bg-slate-200 border rounded-md mt-2 text-black h-14 text-center text-2xl font-bold hover:bg-slate-300 w-full transition-all"
                        onClick={addNewComponent}
                    >
                        Add
                    </button>
                    <div className="text-black flex items-center mt-2">
                        <input
                            type="checkbox"
                            name="another"
                            id="another"
                            defaultChecked={another}
                            disabled={devices.length <= 1}
                            onChange={() => {
                                setAnother(!another);
                            }}
                        />{" "}
                        <label htmlFor="another" className="ml-2 select-none">
                            Add Another Device
                        </label>
                    </div>
                </div>
            </div>
        </div>
    );
}

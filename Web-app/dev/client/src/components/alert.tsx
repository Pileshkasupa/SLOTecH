import { useEffect, useState } from "react";

/**
 *
 *
 * @description If closingFunction is not provided, the alert will only be visible once. If the automaticClosingTimeInMs is under 10 Milliseconds, it will be ignored.
 */
export default function Home(props: {
    message: JSX.Element | string;
    trigger: boolean;
    /**
     * @description If closingFunction is not provided, the alert will only be visible once.
     */
    closingFunction?: () => void;
    /**
     * @description If the automaticClosingTimeInMs is under 10 Milliseconds, it will be ignored.
     */
    automaticClosingTimeInMs?: number;
}) {
    const [trigger, setTrigger] = useState(true);
    const [time, setTime] = useState(props.automaticClosingTimeInMs && props.automaticClosingTimeInMs >= 10 ? props.automaticClosingTimeInMs : null);

    const close = () => {
        setTrigger(false);
    };
    useEffect(() => {
        if (props.trigger && time) {
            const interval = setInterval(() => {
                if (time >= 0) {
                    setTime((prevTime) => (prevTime ? prevTime - 10 : 0));
                }
            }, 10);

            const cleanup = () => {
                clearInterval(interval);
            };
            cleanup;
        }
    }, [props.automaticClosingTimeInMs, props.trigger]);

    useEffect(() => {
        if (time === 0) {
            props.closingFunction ? props.closingFunction() : close();
        }
    }, [time]);

    useEffect(() => {
        setTime(props.automaticClosingTimeInMs && props.automaticClosingTimeInMs >= 10 ? props.automaticClosingTimeInMs : null);
    }, [trigger, props.trigger]);

    return trigger === false || props.trigger === false ? (
        <></>
    ) : (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center select-none">
            <div className="bg-white text-black p-4 rounded-lg shadow-lg">
                <p className="text-xs select-none cursor-default">Info:</p>
                <div className="m-1 bg-slate-100 border border-slate-100 p-2 rounded-md select-text">{props.message}</div>
                <div className="flex items-center justify-between">
                    <button
                        onClick={props.closingFunction ? props.closingFunction : close}
                        className=" bg-blue-500 hover:bg-blue-700 text-white font-bold p-2 rounded-md hover:p-3 transition-all m-3 hover:m-2 select-none"
                    >
                        Close
                    </button>
                    {time ? <p className="font-light p-2 ">{(time / 1000).toFixed(1)}</p> : <></>}
                </div>
            </div>
        </div>
    );
}

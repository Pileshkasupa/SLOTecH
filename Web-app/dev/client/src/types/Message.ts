type Message = {
    Id: string;
    Room: string;
    Value: number;
    Unit: string;
    Type: string;
    MessageType: string;
    Title?: string;
    Image?: string;
    TimeStamp?: string;
    Tags?: string[];
};

export default Message;

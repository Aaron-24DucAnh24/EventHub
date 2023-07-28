import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { IEvent } from "../Dto/IEvent";
import CardEvent from "./CardEvent";

interface Props {
  events: IEvent[];
}
const EventsList = ({ events }: Props) => {
  console.log(events);

  const [eventsList, setEventsList] = useState<IEvent[]>([]);

  useEffect(() => {
    setEventsList(events);
  }, [events]);

  return (
    <div className="flex gap-[24px] flex-wrap">
      {eventsList.map((event, eventIndex) => {
        if (event.isPublished === false)
          return (
            <CardEvent
              key={event.id}
              event={event}
              eventsList={eventsList}
              setEventsList={setEventsList}
            />
          );
        else
          return (
            <Link
              to={`/event/${event.id}`}
              key={event.id}
              className="no-underline"
            >
              <CardEvent
                event={event}
                eventsList={eventsList}
                setEventsList={setEventsList}
              />
            </Link>
          );
      })}
    </div>
  );
};

export default EventsList;

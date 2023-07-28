import { CircularProgress, Container } from "@mui/material";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import api from "../api";
import EventDescription from "../components/EventDescription";
import SeatDescription from "../components/SeatDescription";
import SeatsList from "../components/SeatsList";
import { IEvent } from "../Dto/IEvent";
import { ISeatEvent } from "../Dto/ISeat";

const EventDetailPage = () => {
  const { eventId } = useParams<{ eventId: string }>();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [pickingSeatsCount, setPickingSeatsCount] = useState<number>(0);
  const [event, setEvent] = useState<IEvent>();
  const [seatsList, setSeatsList] = useState<ISeatEvent[] | undefined>(
    event?.seatEvents
  );

  useEffect(() => {
    fetchEvent();
  }, [eventId]);

  useEffect(() => {
    setSeatsList(event?.seatEvents);
  }, [event]);

  const fetchEvent = async () => {
    try {
      setIsLoading(true);
      const response = await api.get(`/Event/${eventId}`, {
        headers: {
          "ngrok-skip-browser-warning": "true",
        },
      });
      if (response.data) {
        setEvent(response.data);
      }
    } catch (error) {
      console.error("Error fetching data:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Container className="my-[80px]">
      {isLoading ? (
        <div className="flex justify-center">
          <CircularProgress />
          <p>Loading...</p>
        </div>
      ) : seatsList !== undefined ? (
        <div>
          <EventDescription
            event={event}
            seatsList={seatsList}
            setSeatsList={setSeatsList}
          />
          <SeatDescription />
          <SeatsList
            seatsList={seatsList}
            setSeatsList={setSeatsList}
            pickingSeatsCount={pickingSeatsCount}
            setPickingSeatsCount={setPickingSeatsCount}
          />
        </div>
      ) : (
        ""
      )}
    </Container>
  );
};

export default EventDetailPage;

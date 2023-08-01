import { useState, useEffect } from "react";
import { CircularProgress, Container } from "@mui/material";
import EventsList from "../components/EventsList";
import { shallow } from "zustand/shallow";
import api from "../api";
import { IEvent } from "../Dto/IEvent";
import { useSearchStore } from "../store/search";

const HomePage = () => {
  //zustand searchStore
  const [searchString] = useSearchStore(
    (state) => [state.searchString],
    shallow
  );
  const [events, setEvents] = useState<IEvent[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const fetchEvents = async () => {
    try {
      setIsLoading(true);
      const response = await api.get("/Event", {
        params: {
          isPublished: "true",
        },
        headers: {
          "Access-Control-Allow-Origin": "*",
        },
      });
      setEvents(response.data);
    } catch (error) {
      console.error("Error fetching data:", error);
    } finally {
      setIsLoading(false);
    }
  };
  useEffect(() => {
    fetchEvents();
  }, []);

  //handle search
  const filterEventsSearch = events.filter(
    (event) =>
      event.title.toLowerCase().includes(searchString.toLowerCase().trim()) ||
      event.minPrice
        .toString()
        .toLowerCase()
        .includes(searchString.toLowerCase().trim()) ||
      event.date.toLowerCase().includes(searchString.toLowerCase().trim()) ||
      event.city.toLowerCase().includes(searchString.toLowerCase().trim())
  );

  return (
    <Container className="my-[80px] flex items-center justify-start self-start flex-col">
      {isLoading ? (
        <div>
          <CircularProgress />
          <p>Loading...</p>
        </div>
      ) : (
        <EventsList events={filterEventsSearch} />
      )}
    </Container>
  );
};

export default HomePage;

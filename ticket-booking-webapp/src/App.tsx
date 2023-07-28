import { Button } from "@mui/material";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { Route, Routes } from "react-router-dom";
import "./App.css";
import Layout from "./components/Layout";
import CreateEventPage from "./pages/CreateEventPage";
import EventDetailPage from "./pages/EventDetailPage";
import HomePage from "./pages/HomePage";
import MyBookingPage from "./pages/MyBookingPage";
import RequireInfo from "./pages/RequireInfo";
import StatisticPage from "./pages/StatisticPage";
import UnpublishedEvent from "./pages/UnpublishedEvent";

function App() {
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/event/:eventId" element={<EventDetailPage />} />
          <Route path="/statistic" element={<StatisticPage />} />
          <Route path="/my-booking/:email" element={<MyBookingPage />} />
          <Route path="/create-event" element={<CreateEventPage />} />
          <Route path="/unpublished-event" element={<UnpublishedEvent />} />
          <Route path="/require-info" element={<RequireInfo />} />
        </Route>
      </Routes>
    </LocalizationProvider>
  );
}

export default App;

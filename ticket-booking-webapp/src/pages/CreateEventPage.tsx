import {
  Alert,
  Box,
  Button,
  Chip,
  CircularProgress,
  Container,
  FormControl,
  InputLabel,
  MenuItem,
  OutlinedInput,
  Select,
  SelectChangeEvent,
  TextField,
  ThemeProvider,
} from "@mui/material";
import FormData from "form-data";
import { createTheme } from "@mui/material/styles";
import { DatePicker } from "@mui/x-date-pickers";
import axios from "axios";
import { ChangeEvent, useEffect, useState } from "react";
import api from "../api";
import { PROVINCE_API } from "../config";
import { IProvince } from "../Dto/IProvince";
import { formatDateToStringCreateEvent } from "../utils/convertDateEvent";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";

const initEventTypes = [
  "Live Music",
  "Art & Culture",
  "Theater & Plays",
  "Nightlife",
  "Outdoor",
  "Conference",
  "Seminar/Courses",
  "Exhibitions",
  "Meetups",
  "Sports",
  "Community",
  "Attractions",
];

const theme = createTheme({
  palette: {
    primary: {
      main: "#5669FF",
    },
  },
});

const CreateEventPage = () => {
  const navigate = useNavigate();
  const [eventDate, setEventDate] = useState<Date>();
  const [city, setCity] = useState<string>("");
  const [eventTypes, setEventTypes] = useState<string[]>([]);

  const [provinces, setProvinces] = useState<IProvince[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [isPostSuccess, setIsPostSuccess] = useState<boolean>(false);
  const [errDatePicker, setErrDatePicker] = useState<string>("");

  const handleDateChange = (date: Date | null) => {
    if (!date) {
      setErrDatePicker("Date is required");
      return;
    }
    const currentDate = new Date();
    const formattedSelectedDate = Date.parse(date.toString());
    const isDateValid = currentDate.getTime() < formattedSelectedDate;
    if (!isDateValid) {
      setErrDatePicker("Selected date must be after or equal to today");
      return;
    }
    setErrDatePicker("");
    setEventDate(date);
  };

  //Fetch Provinces
  const fetchProvinces = async () => {
    try {
      setIsLoading(true);
      const response = await axios.get(PROVINCE_API);
      const newProvinces = response.data.map((province: IProvince) => {
        const newProvince = { ...province };
        newProvince.name = newProvince.name.replace("Tỉnh ", "");
        newProvince.name = newProvince.name.replace("Thành phố ", "");
        return newProvince;
      });
      setProvinces(newProvinces);
      //delete prefix tinh, thanh pho in province name
    } catch (error) {
      console.error("Error fetching data:", error);
    } finally {
      setIsLoading(false);
    }
  };
  useEffect(() => {
    fetchProvinces();
  }, []);

  const handleChangeCity = (event: SelectChangeEvent) => {
    setCity(event.target.value);
  };

  //Handle Change type Event
  const handleChangeEventType = (
    event: SelectChangeEvent<typeof eventTypes>
  ) => {
    const {
      target: { value },
    } = event;
    setEventTypes(
      // On autofill we get a stringified value.
      typeof value === "string" ? value.split(",") : value
    );
  };

  //Handle upload file image
  const [selectedImage, setSelectedImage] = useState<File>();

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      if (!e.target.files[0]) return;
      setSelectedImage(e.target.files[0]);
    }
  };

  //React hook form
  interface FormValues {
    title: string;
    standardPrice: number;
    vipPrice: number;
    sweetboxPrice: number;
    stageName: string;
    location: string;
    duration: string;
  }

  const form = useForm<FormValues>({
    defaultValues: {
      title: "",
      standardPrice: 0,
      vipPrice: 0,
      sweetboxPrice: 0,
      stageName: "",
      location: "",
      duration: "",
    },
  });

  const { register, handleSubmit, formState } = form;
  const { errors } = formState;

  const onSubmit = async (data: FormValues) => {
    if (!eventDate) {
      alert("Date is required");
      return;
    }
    if (errDatePicker) {
      alert(errDatePicker);
      return;
    }
    if (!city) {
      alert("City is required");
      return;
    }
    if (eventTypes.length === 0) {
      alert("Event Type is required");
      return;
    }
    try {
      setIsLoading(true);
      const config = {
        headers: {
          "Content-Type": "multipart/form-data",
          "ngrok-skip-browser-warning": "true",
        },
      };
      let formData = new FormData();
      if (!selectedImage) {
        alert("Image is required");
        return;
      }
      formData.append("Image", selectedImage);
      formData.append("Title", data.title);
      formData.append("Duration", data.duration);
      formData.append(
        "Date",
        formatDateToStringCreateEvent(eventDate.toISOString())
      );
      formData.append("StageName", data.stageName);
      formData.append("Location", data.location);
      formData.append("City", city);
      eventTypes.forEach((eventType) => {
        formData.append("Categories", eventType);
      });
      formData.append("Prices", data.standardPrice);
      formData.append("Prices", data.vipPrice);
      formData.append("Prices", data.sweetboxPrice);
      const response = await api.post("/Event", formData, config);
      if (response) {
        navigate("/unpublished-event");
        setIsPostSuccess(true);
      }
    } catch (error) {
      console.log(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Container className="mt-[80px] mb-[80px]">
      {isPostSuccess && (
        <Alert severity="success" className="mb-[20px]">
          Create event successfully!
        </Alert>
      )}
      {isLoading ? (
        <div className="flex justify-center">
          <CircularProgress />
          <p>Loading...</p>
        </div>
      ) : (
        <form
          className="flex flex-col"
          noValidate
          onSubmit={handleSubmit(onSubmit)}
        >
          <div className="flex flex-row">
            <div className="flex flex-[7] flex-col gap-[20px]">
              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">Title</p>
                <ThemeProvider theme={theme}>
                  <TextField
                    label="Title"
                    variant="outlined"
                    style={{ width: "400px" }}
                    type="text"
                    {...register("title", {
                      required: "Title is required",
                    })}
                    error={!!errors.title}
                    helperText={errors.title?.message}
                  />
                </ThemeProvider>
              </div>
              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">Standard Price</p>
                <ThemeProvider theme={theme}>
                  <TextField
                    label="Standard Price"
                    variant="outlined"
                    type="number"
                    className="w-[400px]"
                    {...register("standardPrice", {
                      required: "Standard Price is required",
                    })}
                    value={
                      form.watch("standardPrice") === 0
                        ? ""
                        : form.watch("standardPrice")
                    }
                    error={!!errors.standardPrice}
                    helperText={errors.standardPrice?.message}
                  />
                </ThemeProvider>
              </div>
              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">VIP Price</p>
                <ThemeProvider theme={theme}>
                  <TextField
                    label="VIP Price"
                    variant="outlined"
                    type="number"
                    className="w-[400px]"
                    {...register("vipPrice", {
                      required: "VIP Price is required",
                    })}
                    value={
                      form.watch("vipPrice") === 0 ? "" : form.watch("vipPrice")
                    }
                    error={!!errors.vipPrice}
                    helperText={errors.vipPrice?.message}
                  />
                </ThemeProvider>
              </div>
              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">Sweetbox Price</p>
                <ThemeProvider theme={theme}>
                  <TextField
                    label="Sweetbox Price"
                    variant="outlined"
                    type="number"
                    className="w-[400px]"
                    {...register("sweetboxPrice", {
                      required: "Sweet Box Price is required",
                    })}
                    value={
                      form.watch("sweetboxPrice") === 0
                        ? ""
                        : form.watch("sweetboxPrice")
                    }
                    error={!!errors.sweetboxPrice}
                    helperText={errors.sweetboxPrice?.message}
                  />
                </ThemeProvider>
              </div>
              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">Stage Name</p>
                <ThemeProvider theme={theme}>
                  <TextField
                    label="Stage Name"
                    variant="outlined"
                    style={{ width: "400px" }}
                    type="text"
                    {...register("stageName", {
                      required: "Stage Name is required",
                    })}
                    error={!!errors.stageName}
                    helperText={errors.stageName?.message}
                  />
                </ThemeProvider>
              </div>

              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">Location</p>
                <ThemeProvider theme={theme}>
                  <TextField
                    label="Location"
                    variant="outlined"
                    style={{ width: "400px" }}
                    type="text"
                    {...register("location", {
                      required: "Location is required",
                    })}
                    error={!!errors.location}
                    helperText={errors.location?.message}
                  />
                </ThemeProvider>
              </div>
              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">Duration</p>
                <ThemeProvider theme={theme}>
                  <TextField
                    label="Duration"
                    variant="outlined"
                    style={{ width: "400px" }}
                    type="text"
                    {...register("duration", {
                      required: "Duration is required",
                    })}
                    error={!!errors.duration}
                    helperText={errors.duration?.message}
                  />
                </ThemeProvider>
              </div>
              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">Event Date</p>
                <ThemeProvider theme={theme}>
                  <DatePicker
                    className="w-[400px]"
                    onChange={handleDateChange}
                  />
                </ThemeProvider>
              </div>
            </div>
            <div className="flex-[1]"></div>
            <div className="flex flex-col flex-[7] gap-[20px]">
              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">City</p>
                <ThemeProvider theme={theme}>
                  <FormControl className="w-[400px]">
                    <InputLabel id="city-select-label">City</InputLabel>
                    <Select
                      labelId="city-select-label"
                      id="city-select"
                      value={city}
                      label="City"
                      onChange={handleChangeCity}
                      MenuProps={{
                        style: {
                          maxHeight: 400,
                        },
                      }}
                    >
                      {provinces.map((province, provinceIndex) => (
                        <MenuItem key={province.code} value={province.name}>
                          {province.name}
                        </MenuItem>
                      ))}
                    </Select>
                  </FormControl>
                </ThemeProvider>
              </div>
              <div className="flex items-center justify-between">
                <p className="my-0 text-[20px]">Event Type</p>
                <ThemeProvider theme={theme}>
                  <FormControl className="w-[400px]">
                    <InputLabel id="event-type-select-label">
                      Event Type
                    </InputLabel>
                    <Select
                      labelId="event-type-select-label"
                      id="event-type-select"
                      label="Event Type"
                      multiple
                      value={eventTypes}
                      onChange={handleChangeEventType}
                      MenuProps={{
                        style: {
                          maxHeight: 300,
                        },
                      }}
                      input={
                        <OutlinedInput id="select-multiple-chip" label="Chip" />
                      }
                      renderValue={(selected) => (
                        <Box
                          sx={{ display: "flex", flexWrap: "wrap", gap: 0.5 }}
                        >
                          {selected.map((value) => (
                            <Chip key={value} label={value} />
                          ))}
                        </Box>
                      )}
                    >
                      {initEventTypes.map((eventType, eventIndex) => (
                        <MenuItem key={eventType} value={eventType}>
                          {eventType}
                        </MenuItem>
                      ))}
                    </Select>
                  </FormControl>
                </ThemeProvider>
              </div>
              <ThemeProvider theme={theme}>
                <Button
                  variant="contained"
                  component="label"
                  className="flex self-start"
                >
                  Upload File
                  <input
                    type="file"
                    id="imgUploadLabel"
                    accept="image/*"
                    hidden
                    onChange={handleFileChange}
                  />
                </Button>
              </ThemeProvider>
              {selectedImage && (
                <div className="w-full">
                  <label htmlFor="imgUploadLabel" className="w-full">
                    <img
                      alt="imageUpload"
                      className="w-full cursor-pointer hover:opacity-80"
                      style={{
                        boxShadow:
                          "rgba(0, 0, 0, 0.19) 0px 10px 20px, rgba(0, 0, 0, 0.23) 0px 6px 6px",
                      }}
                      src={URL.createObjectURL(selectedImage)}
                    />
                  </label>
                </div>
              )}
            </div>
          </div>
          <div className="mt-[40px] flex justify-center">
            <ThemeProvider theme={theme}>
              <Button
                variant="contained"
                type="submit"
                className="flex w-[400px] text-[16px] py-[10px] rounded-[10px]"
              >
                Create Event
              </Button>
            </ThemeProvider>
          </div>
        </form>
      )}
    </Container>
  );
};

export default CreateEventPage;

import * as React from "react";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import CardMedia from "@mui/material/CardMedia";
import Typography from "@mui/material/Typography";
import {
  Button,
  CardActionArea,
  CircularProgress,
  createTheme,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  ThemeProvider,
} from "@mui/material";
import { SlCalender } from "react-icons/sl";
import { IEvent } from "../Dto/IEvent";
import { formatDate } from "../utils/convertDateEvent";
import { useState } from "react";
import api from "../api";

interface Props {
  event: IEvent;
  eventsList?: IEvent[];
  setEventsList?: React.Dispatch<React.SetStateAction<IEvent[]>>;
}

const theme = createTheme({
  palette: {
    primary: {
      main: "#5669FF",
    },
  },
});

const CardEvent = ({ event, eventsList, setEventsList }: Props) => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [openPublishConfirmation, setOpenPublishConfirmation] =
    useState<boolean>(false);
  const handleClosePublishConfirmation = () =>
    setOpenPublishConfirmation(false);

  const hanldeSubmitPublishConfirmation = async () => {
    try {
      setIsLoading(true);
      const response = await api.put(`/Event/${event.id}`, {
        params: {
          eventId: event.id,
        },
        headers: {
          "Access-Control-Allow-Origin": "*",
        },
      });
      if (response && eventsList && setEventsList) {
        const newEventsList = eventsList.filter(
          (eventItem) => eventItem.id !== event.id
        );
        setEventsList(newEventsList);
      }
      setIsLoading(false);
      setOpenPublishConfirmation(false);
    } catch (error) {
      console.log(error);
      setIsLoading(false);
      setOpenPublishConfirmation(false);
    } finally {
    }
  };

  const handleClickPublish = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    setOpenPublishConfirmation(true);
  };
  return (
    <Card sx={{ maxWidth: 345, height: "100%" }}>
      <CardActionArea>
        <CardMedia
          component="img"
          height="140"
          image={event.image}
          alt="green iguana"
        />
        <CardContent>
          <Typography
            gutterBottom
            variant="h5"
            component="div"
            className="text-[18px] font-[500] leading-[24px] h-[72px] hover:text-main uppercase"
          >
            {event.title}
          </Typography>
          <div className="flex justify-between gap-[20px]">
            <Typography variant="body2" color="text.secondary">
              From{" "}
              <span className="text-main font-[700]">{`${event.minPrice.toLocaleString()} VND`}</span>
            </Typography>
            <div className="flex gap-[6px] items-center">
              <SlCalender className="w-[16px] h-[16px] text-main" />
              <Typography variant="body2" color="text.secondary">
                {formatDate(event.date)}
              </Typography>
            </div>
          </div>
          <div className="flex mt-[12px] justify-between items-center">
            <div className="flex">
              {event.categories.map((category, categoryIndex) => (
                <Typography
                  key={category.id}
                  color="text.secondary"
                  className="pr-[10px] text-[12px]"
                >
                  {category.name}
                </Typography>
              ))}
            </div>
            {event.city ? (
              <div
                style={{ border: "1px solid #ccc" }}
                className="border-2 border-[#333] px-[4px] py-[2px] rounded-[2px]"
              >
                <Typography
                  variant="body2"
                  color="text.secondary"
                  className="text-[10px]"
                >
                  {event.city}
                </Typography>
              </div>
            ) : (
              ""
            )}
          </div>

          {!event.isPublished ? (
            <div className="flex">
              <Button
                variant="contained"
                className="rounded-[6px] bg-main text-[#fff] text-[12px] px-[16px] py-[6px] mt-[6px] self-center"
                style={{ textTransform: "none" }}
                onClick={handleClickPublish}
              >
                Publish
              </Button>

              <ThemeProvider theme={theme}>
                {isLoading ? (
                  <div>
                    <CircularProgress />
                    <p>Loading...</p>
                  </div>
                ) : (
                  <Dialog
                    open={openPublishConfirmation}
                    onClose={handleClosePublishConfirmation}
                  >
                    <DialogTitle
                      className="text-main"
                      style={{ color: "#5669FF", fontSize: "24px" }}
                    >
                      Publish Confirmation
                    </DialogTitle>
                    <DialogContent
                      style={{
                        height: "100%",
                        position: "relative",
                        display: "flex",
                        flexDirection: "column",
                      }}
                    >
                      <DialogContentText>
                        {`Are you sure you want to publish ${event.title} event? If yes, please confirm below.`}
                      </DialogContentText>
                    </DialogContent>
                    <DialogActions>
                      <Button onClick={handleClosePublishConfirmation}>
                        Cancel
                      </Button>
                      <Button onClick={hanldeSubmitPublishConfirmation}>
                        Confirm
                      </Button>
                    </DialogActions>
                  </Dialog>
                )}
              </ThemeProvider>
            </div>
          ) : (
            ""
          )}
        </CardContent>
      </CardActionArea>
    </Card>
  );
};
export default CardEvent;

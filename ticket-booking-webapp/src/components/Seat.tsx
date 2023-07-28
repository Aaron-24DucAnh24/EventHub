import classNames from "classnames";
import { ISeatEvent } from "../Dto/ISeat";

const STANDARD = 0;
const VIP = 1;
const SWEETBOX = 2;
const DEFAULT = 0;
const PICKING = -1;
const PICKED = 1;
const BANNED = 2;

interface Props {
  typeSeat: number;
  seatStatus: number;
  seatId: string;
  seatsList: ISeatEvent[];
  setSeatsList: (seatsList: ISeatEvent[]) => void;
  pickingSeatsCount: number;
  setPickingSeatsCount: (pickingSeatsCount: number) => void;
}

const Seat = ({
  typeSeat,
  seatStatus,
  seatId,
  seatsList,
  setSeatsList,
  pickingSeatsCount,
  setPickingSeatsCount,
}: Props) => {
  const handleClickSeat = () => {
    const seatPickingIndex = seatsList.findIndex(
      (seat) => seat.seatId === seatId
    );
    //Update SeatsList status picking
    if (seatPickingIndex === -1) {
      return;
    }
    const updatedSeats = [...seatsList];
    if (
      updatedSeats[seatPickingIndex].seatStatus === PICKED ||
      updatedSeats[seatPickingIndex].seatStatus === BANNED
    ) {
      return;
    }
    if (updatedSeats[seatPickingIndex].seatStatus === PICKING) {
      updatedSeats[seatPickingIndex].seatStatus = DEFAULT;
      setPickingSeatsCount(pickingSeatsCount - 1);
    } else if (pickingSeatsCount < 3) {
      updatedSeats[seatPickingIndex].seatStatus = -1;
      setPickingSeatsCount(pickingSeatsCount + 1);
    } else {
      alert("You cannot select more than 3 tickets!");
      return;
    }
    setSeatsList(updatedSeats);
  };

  return (
    <div
      className={classNames(
        "w-[40px] h-[40px] border-[2px] border-solid rouned-[2px] flex justify-center items-center cursor-pointer hover:opacity-80",
        {
          "bg-[#ccc] border-[#ccc] text-[#fff]":
            seatStatus === BANNED || seatStatus === PICKED,
          "bg-[#88050c] border-[#88050c] text-[#fff]": seatStatus === PICKING,
          "border-main": seatStatus === DEFAULT && typeSeat === STANDARD,
          "border-[#e50914]": seatStatus === DEFAULT && typeSeat === VIP,
          "border-[#ee7a7a] bg-[#ee7a7a] text-[#fff]":
            seatStatus === DEFAULT && typeSeat === SWEETBOX,
        }
      )}
      onClick={handleClickSeat}
    >
      <p
        className={classNames("m-0", {
          "text-[24px]": seatStatus === BANNED,
        })}
      >
        {seatStatus === BANNED ? "X" : seatId}
      </p>
    </div>
  );
};

export default Seat;

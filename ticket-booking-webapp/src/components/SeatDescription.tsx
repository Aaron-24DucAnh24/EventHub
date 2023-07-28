import classNames from "classnames";

interface PropsASeatDescription {
  typeSeat: string;
  statusSeat: string;
}

const PICKED = "picked";
const BANNED = "banned";
const PICKING = "picking";
const DEFAULT = "default";
const VIP = "vip";
const STANDARD = "standard";
const SWEETBOX = "sweet-box";

const ASeatDescription = ({ typeSeat, statusSeat }: PropsASeatDescription) => {
  return (
    <div className="flex items-center gap-[12px]">
      <div
        className={classNames(
          "w-[40px] h-[40px] border-[2px] rouned-[2px] border-solid flex justify-center items-center cursor-pointer hover:opacity-80",
          {
            "bg-picking border-picking text-[#fff]": statusSeat === PICKING,
            "bg-picked border-picked text-[#fff]":
              statusSeat === BANNED || statusSeat === PICKED,
            "border-main": statusSeat === DEFAULT && typeSeat === STANDARD,
            "border-vip": statusSeat === DEFAULT && typeSeat === VIP,
            "border-sweetbox bg-sweetbox text-[#fff]":
              statusSeat === DEFAULT && typeSeat === SWEETBOX,
          }
        )}
      >
        {statusSeat === BANNED ? <p className="text-[20px]">X</p> : ""}
      </div>
      <p className="m-0 text-[18px]">
        {statusSeat === PICKING
          ? "Picking"
          : statusSeat === BANNED
          ? "Banned"
          : statusSeat === PICKED
          ? "Picked"
          : typeSeat === STANDARD
          ? "Standard"
          : typeSeat === VIP
          ? "VIP"
          : typeSeat === SWEETBOX
          ? "Sweet Box"
          : ""}
      </p>
    </div>
  );
};

const SeatDescription = () => {
  return (
    <div className="my-[80px] flex justify-between">
      <div className="flex gap-[36px]">
        <ASeatDescription typeSeat={VIP} statusSeat={PICKING} />
        <ASeatDescription typeSeat={VIP} statusSeat={PICKED} />
        <ASeatDescription typeSeat={VIP} statusSeat={BANNED} />
      </div>
      <div className="flex gap-[36px]">
        <ASeatDescription typeSeat={STANDARD} statusSeat={DEFAULT} />
        <ASeatDescription typeSeat={VIP} statusSeat={DEFAULT} />
        <ASeatDescription typeSeat={SWEETBOX} statusSeat={DEFAULT} />
      </div>
    </div>
  );
};

export default SeatDescription;

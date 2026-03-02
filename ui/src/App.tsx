import "./App.css";
import { useTable } from "spacetimedb/react";
import { tables } from "./module_bindings";

function App() {
  const [activeRounds] = useTable(tables.ActiveRound);
  const activeRound = activeRounds[0];

  if (!activeRound) return null;

  return (
    <div>
      <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
        <div
          style={{
            display: "flex",
            flexDirection: "row",
            justifyContent: "space-between",
            gap: 4,
          }}
        >
          <span>Status:</span>
          <span>{activeRound.status.tag}</span>
        </div>

        <div
          style={{
            display: "flex",
            flexDirection: "row",
            justifyContent: "space-between",
            gap: 4,
          }}
        >
          {activeRound.status.tag === "Countdown" ? (
            <>
              <span>Countdown:</span>
              <span>{activeRound.countdownSecondsRemaining}</span>
            </>
          ) : (
            <>
              <span>Multiplier:</span>
              <span>
                {activeRound.currentMultiplier?.toFixed(2) ?? "0.00"}x
              </span>
            </>
          )}
        </div>
      </div>
    </div>
  );
}

export default App;

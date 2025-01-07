import { faGithub } from "@fortawesome/free-brands-svg-icons"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import { Line } from "react-chartjs-2"
import axios from 'axios'
import 'chart.js/auto'
import { useState } from "react"

interface PriceHistoryResponse {
  symbol: string,
  history: number[],
}

function App() {
  const [symbol, setSymbol] = useState('')
  const [label, setLabel] = useState('')
  const [history, setHistory] = useState<number[]>([])

  async function getPriceHistory() {
    const res = await axios.get<PriceHistoryResponse>(`${import.meta.env.VITE_BACKEND_URL}/stocks/${symbol}`)
    setHistory(res.data.history)
    setLabel(symbol)
  }

  async function onInputKeyDown(event: React.KeyboardEvent<HTMLInputElement>) {
    if (event.key == 'Enter') {
      await getPriceHistory()
    }
  }

  return (
    <>
      <div className="px-8 p-2 border-b border-gray-200 bg-gray-100">
        <div className="flex justify-between items-center">
          <h1 className="font-bold text-xl">MockStocks</h1>
          <FontAwesomeIcon className="text-4xl" icon={faGithub}/>
        </div>
      </div>
      <div className="px-8 py-4">
        <div className="px-8 py-4">
          <input
            type="text"
            className="px-4 py-2 border border-r-0 rounded-l-sm outline-none"
            placeholder="Symbol"
            value={symbol}
            onKeyDown={e => onInputKeyDown(e)}
            onChange={e => setSymbol(e.target.value)}
          />
          <button
            className="px-4 py-2 bg-green-700 text-white rounded-r-sm"
            onClick={getPriceHistory}>
              Fetch
          </button>
        </div>
        <div style={{maxWidth: '1000px', height: '1000px'}}>
          <Line
            options={
              {
                plugins: {
                  title: {
                    display: true,
                    text: "History"
                  }
                },
                scales: {
                  x: {
                    title: {
                      display: true,
                      text: 'Date'
                    }
                  },
                  y: {
                    title: {
                      display: true,
                      text: 'Price'
                    }
                  }
                }
              }
            }
            datasetIdKey='0'
            data={{
              labels: Array.from(Array(history.length).keys()),
              datasets: [
                {
                  label: label.length == 0 ? 'No symbol selected' : `$${label.toUpperCase()}`,
                  data: history,
                },
              ],
            }}
          />
        </div>
      </div>
    </>
  )
}

export default App

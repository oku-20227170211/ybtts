import React from 'react'

type Props = React.InputHTMLAttributes<HTMLInputElement> & {
  label?: string
}

const Input: React.FC<Props> = ({ label, className = '', ...rest }) => {
  return (
    <label className="flex flex-col gap-1">
      {label && <span className="text-sm text-gray-600">{label}</span>}
      <input className={`border px-3 py-2 rounded ${className}`} {...rest} />
    </label>
  )
}

export default Input

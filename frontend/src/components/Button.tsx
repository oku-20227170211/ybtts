import React from 'react'

type Props = React.ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: 'primary' | 'secondary'
}

const Button: React.FC<Props> = ({ children, variant = 'primary', className = '', ...rest }) => {
  const base = 'px-4 py-2 rounded text-white'
  const vclass = variant === 'primary' ? 'bg-blue-600 hover:bg-blue-700' : 'bg-gray-500 hover:bg-gray-600'
  return (
    <button className={`${base} ${vclass} ${className}`} {...rest}>
      {children}
    </button>
  )
}

export default Button

/* eslint-disable @typescript-eslint/no-unused-vars */
import { useState } from 'react'
// import { signupOptStyles as styles } from './styles/signupOptStyles'
import { useNavigate } from 'react-router-dom'

import { AuthButton } from '@/components/auth/auth-button.tsx'
import { EmailIcon, GithubIcon, GoogleIcon } from '@/components/icons/index.tsx'

import styles from './styles/signup-options.module.css'

export default function SignupOptions() {
    const navigate = useNavigate()
    const [isLoading, setIsLoading] = useState(false)

    const handleEmailSignup = () => {
        setIsLoading(true)
        navigate('/email-registration')
    }

    const handleGoogleSignup = () => {
        setIsLoading(true)
        // Add Google signup logic
    }

    const handleGithubSignup = () => {
        setIsLoading(true)
        // Add Github signup logic
    }

    return (
        <div className={styles.container}>
            <div className={styles.card}>
                {/* Header */}
                <div className={styles.headerContainer}>
                    <h1 className={styles.headerTitle}>Create Account</h1>
                    <p className={styles.headerSubtitle}>Choose how you&apos;d like to sign up</p>
                </div>
                {/* <div className={styles.header.container}>
                    <h1 className={styles.header.title}>Create Account</h1>
                    <p className={styles.header.subtitle}>Choose how you'd like to sign up</p>
                </div> */}

                {/* Signup Options */}
                <div className={styles.options}>
                    <AuthButton 
                        disabled={isLoading}
                        icon={<EmailIcon />}
                        onClick={handleEmailSignup}
                    >
                        Continue with Email
                    </AuthButton>

                    <AuthButton 
                        disabled={true}
                        icon={<GoogleIcon />}
                    >
                        Continue with Google (Coming Soon)
                    </AuthButton>

                    <AuthButton 
                        disabled={true}
                        icon={<GithubIcon />}
                    >
                        Continue with Github (Coming Soon)
                    </AuthButton>
                </div>

                {/* Login Link */}
                <div className={styles.loginLinkContainer}>
                    <p className={styles.loginLinkText}>
                        Already have an account?{' '}
                        <button 
                            className={styles.loginLinkButton}
                            onClick={() => navigate('/login')}
                        >
                            Log in
                        </button>
                    </p>
                </div>
                {/* <div className={styles.loginLink.container}>
                    <p className={styles.loginLink.text}>
                        Already have an account?{' '}
                        <button 
                            onClick={() => navigate('/Login')}
                            className={styles.loginLink.button}
                        >
                            Log in
                        </button>
                    </p>
                </div> */}
            </div>
        </div>
    )
}